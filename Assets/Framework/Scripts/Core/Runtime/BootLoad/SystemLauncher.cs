using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal class SystemLauncher : IBootLoadElement, IEventReceiver<ExecutableSystemMethodID>
    {
        private readonly SystemExecuteRepository _systemExecuteRepository;
        private readonly StateMachine _stateMachine;
        private readonly SystemRegister _systemRegister;
        private readonly MonoEventsTransmitter _transmitter;

        private readonly ExecutableSystemMethodID[] _initializeMethodsEnabled;
        private readonly ExecutableSystemMethodID[] _initializeMethodsDisabed;

        internal SystemLauncher(SystemExecuteRepository systemExecuteRepository)
        {
            _systemExecuteRepository = systemExecuteRepository;
            _stateMachine = LoadElementAdapter<StateMachine>.Instance;
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;
            _transmitter = LoadElementAdapter<MonoEventsTransmitter>.Instance;

            _initializeMethodsEnabled = new ExecutableSystemMethodID[] 
            { 
                ExecutableSystemMethodID.OnInitialize,
                ExecutableSystemMethodID.OnEnable,
                ExecutableSystemMethodID.OnBegin, 
            };

            _initializeMethodsDisabed = new ExecutableSystemMethodID[] 
            { 
                ExecutableSystemMethodID.OnInitialize, 
                ExecutableSystemMethodID.OnBegin,
            };

            _systemRegister.OnModuleRegister += RegisterModuleCallbacks;
            _systemRegister.OnModuleUnregister += UnregisterModuleCallbacks;
            _transmitter.OnInitialize += OnTransmitterInitialize;
            _transmitter.Subscribe(this);
        }

        private void OnTransmitterInitialize()
        {
            _transmitter.OnInitialize -= OnTransmitterInitialize;
            _systemRegister.OnModuleRegister += CallStartModuleEvents;
        }

        public void OnReceive(ExecutableSystemMethodID arg)
        {
            ExecuteCommon(arg, _stateMachine.CurrentState);
        }
        
        private void RegisterModuleCallbacks(SystemModule systemModule)
        {
            systemModule.OnModuleEnable.AddDelegate(OnModuleEnable);
            systemModule.OnModuleDisable.AddDelegate(OnModuleDisable);
            systemModule.OnModuleDestroy.AddDelegate(OnModuleDestroy);
        }

        private void UnregisterModuleCallbacks(SystemModule systemModule)
        {
            systemModule.OnModuleEnable.RemoveDelegate(OnModuleEnable);
            systemModule.OnModuleDisable.RemoveDelegate(OnModuleDisable);
            systemModule.OnModuleDestroy.RemoveDelegate(OnModuleDestroy);
        }

        private void CallStartModuleEvents(SystemModule systemModule)
        {
            ExecutableSystemMethodID[] methods = systemModule.gameObject.activeSelf ?
                _initializeMethodsEnabled : _initializeMethodsDisabed;

            foreach (ExecutableSystemMethodID methodID in methods)
                ExecuteSpecific(methodID, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleEnable(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableSystemMethodID.OnEnable, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleDisable(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableSystemMethodID.OnDisable, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleDestroy(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableSystemMethodID.OnDestroy, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void ExecuteCommon(ExecutableSystemMethodID methodID, int stateIndex)
        {
            _systemExecuteRepository.GetStatedExecuteHandler(methodID, stateIndex)?.Execute();
            _systemExecuteRepository.GetNotStatedExecuteHandler(methodID)?.Execute();
        }

        private void ExecuteSpecific(ExecutableSystemMethodID methodID, int stateIndex, IEnumerable<IExecutable> targets)
        {
            _systemExecuteRepository.GetStatedExecuteHandler(methodID, stateIndex)?.ExecuteSpecific(targets);
            _systemExecuteRepository.GetNotStatedExecuteHandler(methodID)?.ExecuteSpecific(targets);
        }

        public void OnBootLoadComplete() { }
    }
}