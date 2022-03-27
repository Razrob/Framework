using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class SystemModule : MonoBehaviour
    {
        [SerializeField] [SubTypesFilter(new Type[] { typeof(FSystemFoundation) })] private SerializableType[] _attachedSystemsTypes;

        private FSystemFoundation[] _systems;
        private SystemData[] _systemsData;

        public IReadOnlyList<FSystemFoundation> Systems => _systems;
        public IReadOnlyList<SystemData> SystemsData => _systemsData;

        public readonly OrderedExecuteHandler<SystemModule> OnModuleEnable;
        public readonly OrderedExecuteHandler<SystemModule> OnModuleDisable;
        public readonly OrderedExecuteHandler<SystemModule> OnModuleDestroy;

        public SystemModule()
        {
            OnModuleDisable = new OrderedExecuteHandler<SystemModule>();
            OnModuleEnable = new OrderedExecuteHandler<SystemModule>();
            OnModuleDestroy = new OrderedExecuteHandler<SystemModule>();
        }

        private void Awake()
        {
            Type[] systemsTypes = _attachedSystemsTypes.Select(type => type.Type).Where(type => CheckSystemValid(type)).ToArray();
            _systems = new FSystemFoundation[systemsTypes.Length];
            _systemsData = new SystemData[systemsTypes.Length];

            for (int i = 0; i < systemsTypes.Length; i++)
            {
                _systemsData[i] = new SystemData((FSystemFoundation)Activator.CreateInstance(systemsTypes[i]), systemsTypes[i]);
                _systemsData[i].InternalModelFields = InternalModelExtractor.GetInternalModelData(systemsTypes[i]);
                _systemsData[i].InjectionsFields = InjectionsExtractor.GetInjectionsData(systemsTypes[i]);
                _systemsData[i].ComponentSelectorFields = ComponentSelectorExtractor.GetSelectors(systemsTypes[i]);
                _systemsData[i].SystemExecuteData = SystemExecuteDataExtractor.GetExecuteData(systemsTypes[i]);

                _systems[i] = _systemsData[i].System;
            }

            LoadElementAdapter<SystemRegister>.Instance.RegisterSystemModule(this);
        }

        private bool CheckSystemValid(Type systemType) => !(systemType.GetCustomAttribute(typeof(FrameworkSystemAttribute)) is null);

        private void OnEnable() => OnModuleEnable.Execute(this);
        private void OnDisable() => OnModuleDisable.Execute(this);
        private void OnDestroy()
        {
            OnModuleDestroy.Execute(this);

            OnModuleEnable.RemoveAllDelegates();
            OnModuleDisable.RemoveAllDelegates();
            OnModuleDestroy.RemoveAllDelegates();
        }
    }
}