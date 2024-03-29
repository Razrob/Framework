using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Runtime
{
    [DefaultExecutionOrder(-1000)]
    public class BootLoader : MonoBehaviour, IBootLoadElement
    {
        [SerializeField] [SubTypesFilter(new Type[] { typeof(Enum) })] private SerializableType _statesEnum;
        [SerializeField] [SubTypesFilter(new Type[] { typeof(InternalModel) })] private SerializableType[] _modelsToInitializeIntermediate;
        [SerializeField] private UnityEngine.Object[] _injections;

        public const string Version = "0.5f";

        private StatesPreset _statesPreset;
        private SystemExecuteRepository _systemExecuteRepository;
        private FComponentsRepository _componentsRepository;

        private SystemRegister _systemRegister;
        private StateMachine _stateMachine;
        private MonoEventsTransmitter _transmitter;
        private SystemLauncher _systemLauncher;
        private InternalModelInjector _modelInjector;
        private FieldsInjector _fieldsInjector;

        private FEntityRegister _entityRegister;
        private ComponentsSelectorsInjector _componentInjector;

        private void Awake()
        {
            gameObject.name = typeof(BootLoader).Name;
            DontDestroyOnLoad(gameObject);

            _statesPreset = FormStatesPreset(_statesEnum.Type);

            LoadElementAdapter<BootLoader>.Initialize(this);
            _systemRegister = LoadElementAdapter<SystemRegister>.Initialize(new SystemRegister(_statesPreset, out _systemExecuteRepository));
            _stateMachine = LoadElementAdapter<StateMachine>.Initialize(new StateMachine(_statesPreset));
            _transmitter = LoadElementAdapter<MonoEventsTransmitter>.Initialize(gameObject.AddComponent<MonoEventsTransmitter>());
            _systemLauncher = LoadElementAdapter<SystemLauncher>.Initialize(new SystemLauncher(_systemExecuteRepository));

            _entityRegister = LoadElementAdapter<FEntityRegister>.Initialize(new FEntityRegister(out _componentsRepository));
            LoadElementAdapter<FComponentsRepository>.Initialize(_componentsRepository);

            FrameworkEventCommander.Initialize();
        }

        private void Start()
        {
            _modelInjector = LoadElementAdapter<InternalModelInjector>
                .Initialize(new InternalModelInjector(_modelsToInitializeIntermediate.Select(type => type.Type).ToArray()));
            _componentInjector = LoadElementAdapter<ComponentsSelectorsInjector>.Initialize(new ComponentsSelectorsInjector(_componentsRepository));
            _fieldsInjector = LoadElementAdapter<FieldsInjector>.Initialize(new FieldsInjector(_injections));

            Validate();

            CallBootLoadCallback();
        }

        private void CallBootLoadCallback()
        {
            foreach (IBootLoadElement loadElement in LoadElementAdapter.BootLoadElements)
                loadElement.OnBootLoadComplete();
        }

        private void Validate()
        {
            StateValidator stateValidator = new StateValidator(_stateMachine.StatesIndexes, _statesEnum.Type);
            stateValidator.ValidateSystems(_systemRegister.RegisteredModules);
        }

        private StatesPreset FormStatesPreset(Type enumType)
        {
            Array enumValues = Enum.GetValues(enumType);
            int[] indexes = new int[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
                indexes[i] = Convert.ToInt32(enumValues.GetValue(i));

            return new StatesPreset(indexes);
        }

        public void OnBootLoadComplete() { }
    }
}