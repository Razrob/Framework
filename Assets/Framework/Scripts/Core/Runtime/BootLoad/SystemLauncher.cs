using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class SystemLauncher : IBootLoadElement, IEventReceiver<ExecutableMethodID>
    {
        private readonly SystemExecuteRepository _systemExecuteRepository;
        private readonly StateMachine _stateMachine;
        private readonly SystemRegister _systemRegister;
        private readonly MonoEventsTransmitter _transmitter;

        private readonly ExecutableMethodID[] _initializeMethodsEnabled;
        private readonly ExecutableMethodID[] _initializeMethodsDisabed;

        public SystemLauncher(SystemExecuteRepository systemExecuteRepository)
        {
            _systemExecuteRepository = systemExecuteRepository;
            _stateMachine = LoadElementAdapter<StateMachine>.Instance;
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;
            _transmitter = LoadElementAdapter<MonoEventsTransmitter>.Instance;

            _initializeMethodsEnabled = new ExecutableMethodID[] 
            { 
                ExecutableMethodID.OnInitialize,
                ExecutableMethodID.OnEnable,
                ExecutableMethodID.OnBegin, 
            };

            _initializeMethodsDisabed = new ExecutableMethodID[] 
            { 
                ExecutableMethodID.OnInitialize, 
                ExecutableMethodID.OnBegin,
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

        public void OnReceive(ExecutableMethodID arg)
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
            ExecutableMethodID[] methods = systemModule.gameObject.activeSelf ?
                _initializeMethodsEnabled : _initializeMethodsDisabed;

            foreach (ExecutableMethodID methodID in methods)
                ExecuteSpecific(methodID, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleEnable(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableMethodID.OnEnable, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleDisable(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableMethodID.OnDisable, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void OnModuleDestroy(SystemModule systemModule)
        {
            ExecuteSpecific(ExecutableMethodID.OnDestroy, _stateMachine.CurrentState, systemModule.Systems);
        }

        private void ExecuteCommon(ExecutableMethodID methodID, int stateIndex)
        {
            _systemExecuteRepository.GetStatedExecuteHandler(methodID, stateIndex).Execute();
            _systemExecuteRepository.GetNotStatedExecuteHandler(methodID).Execute();
        }

        private void ExecuteSpecific(ExecutableMethodID methodID, int stateIndex, IEnumerable<IExecutable> targets)
        {
            _systemExecuteRepository.GetStatedExecuteHandler(methodID, stateIndex).ExecuteSpecific(targets);
            _systemExecuteRepository.GetNotStatedExecuteHandler(methodID).ExecuteSpecific(targets);
        }
    }
}