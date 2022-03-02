using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class ComponentsSelectorsInjector : IBootLoadElement
    {
        private readonly FComponentsRepository _componentsRepository;

        public ComponentsSelectorsInjector(FComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository;

            SystemRegister systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            foreach (SystemModule systemModule in systemRegister.RegisteredModules)
                InjectSelectors(systemModule);

            systemRegister.OnModuleRegister += InjectSelectors;
        }

        private void InjectSelectors(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.ComponentSelectorFields)
                {
                    field.SetValue(data.System, Activator.CreateInstance(field.FieldType, _componentsRepository));
                }
            }
        }
    }
}