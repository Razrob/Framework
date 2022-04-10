using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Framework.Core.Runtime
{
    public class SystemRegister : IBootLoadElement
    {
        private readonly LinkedList<SystemModule> _registeredModules;
        private readonly LinkedList<FSystemFoundation> _registeredSystems;

        private readonly ExecutableSystemMethodID[] ExecutableMethodsID;
        private readonly ExecutableSystemMethodID[] DisablableExecutableMethodsID;

        private readonly SystemExecuteRepository _systemExecuteRepository;

        public IReadOnlyCollection<SystemModule> RegisteredModules => _registeredModules;
        public IReadOnlyCollection<FSystemFoundation> RegisteredSystems => _registeredSystems;

        public event RegisterDelegate<SystemModule> OnModuleRegister;
        public event RegisterDelegate<SystemModule> OnModuleUnregister;

        public SystemRegister(StatesPreset statesPreset, out SystemExecuteRepository systemExecuteRepository)
        {
            _registeredModules = new LinkedList<SystemModule>();
            _registeredSystems = new LinkedList<FSystemFoundation>();

            Array methodsID = Enum.GetValues(typeof(ExecutableSystemMethodID));
            ExecutableMethodsID = new ExecutableSystemMethodID[methodsID.Length];

            for (int i = 0; i < ExecutableMethodsID.Length; i++)
                ExecutableMethodsID[i] = (ExecutableSystemMethodID)methodsID.GetValue(i);

            DisablableExecutableMethodsID = new ExecutableSystemMethodID[] 
            { 
                ExecutableSystemMethodID.OnBegin, 
                ExecutableSystemMethodID.OnUpdate, 
                ExecutableSystemMethodID.OnFixedUpdate 
            };

            systemExecuteRepository = _systemExecuteRepository = new SystemExecuteRepository(statesPreset.StatesIndexes);
        }

        public void RegisterSystemModule(SystemModule systemModule)
        {
            if (_registeredModules.Contains(systemModule))
                return;

            if (_registeredSystems.Count(system => systemModule.Systems.Contains(system)) > 0)
                throw new Exception("Systems cannot be duplicated. System module was not registered");

            _registeredModules.AddLast(systemModule);
            foreach (FSystemFoundation system in systemModule.Systems)
                _registeredSystems.AddLast(system);

            systemModule.OnModuleEnable.AddDelegate(AppendSystemsExecuteData, -500);
            systemModule.OnModuleDisable.AddDelegate(RemoveDisablableSystemExecuteData, 500);
            systemModule.OnModuleDestroy.AddDelegate(UnregisterSystemModule, 1000);

            OnModuleRegister?.Invoke(systemModule);
        }

        public void UnregisterSystemModule(SystemModule systemModule)
        {
            if (!_registeredModules.Contains(systemModule))
                return;

            _registeredModules.Remove(systemModule);
            foreach (FSystemFoundation system in systemModule.Systems)
                _registeredSystems.Remove(system);

            systemModule.OnModuleEnable.RemoveDelegate(AppendSystemsExecuteData);
            systemModule.OnModuleDisable.RemoveDelegate(RemoveDisablableSystemExecuteData);
            systemModule.OnModuleDestroy.RemoveDelegate(UnregisterSystemModule);

            RemoveAllSystemExecuteData(systemModule);

            OnModuleUnregister?.Invoke(systemModule);
        }

        private void RemoveAllSystemExecuteData(SystemModule systemModule) => RemoveSystemsExecuteData(systemModule, ExecutableMethodsID);
        private void RemoveDisablableSystemExecuteData(SystemModule systemModule) => RemoveSystemsExecuteData(systemModule, DisablableExecutableMethodsID);

        private void AppendSystemsExecuteData(SystemModule systemModule)
        {
            foreach(SystemData data in systemModule.SystemsData)
            {
                foreach (SystemMethodData methodData in data.SystemExecuteData.MethodsData)
                {
                    ExecuteDelegate @delegate = methodData.MethodInfo.CreateDelegate(typeof(ExecuteDelegate), data.System) as ExecuteDelegate;

                    if (methodData.ExecutableAttribute.StateDependency)
                    {
                        foreach (int stateIndex in data.SystemExecuteData.FrameworkSystemAttribute.AttachedStates)
                        {
                            _systemExecuteRepository.GetStatedExecuteHandler(methodData.MethodInfo.Name.ToEnum<ExecutableSystemMethodID>(), stateIndex)
                                .AddDelegate(@delegate, methodData.ExecutableAttribute.ExecutionOrder);
                        }
                    }
                    else
                    {
                        _systemExecuteRepository.GetNotStatedExecuteHandler(methodData.MethodInfo.Name.ToEnum<ExecutableSystemMethodID>())
                                .AddDelegate(@delegate, methodData.ExecutableAttribute.ExecutionOrder);
                    }
                }
            }
        }

        private void RemoveSystemsExecuteData(SystemModule systemModule, ExecutableSystemMethodID[] methodsID)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (ExecutableSystemMethodID methodID in methodsID)
                    foreach (int stateIndex in data.SystemExecuteData.FrameworkSystemAttribute.AttachedStates)
                        _systemExecuteRepository.GetStatedExecuteHandler(methodID, stateIndex).RemoveTarget(data.System);
            }
        }

        public void OnBootLoadComplete() { }
    }
}