using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class FieldsInjector : IBootLoadElement
    {
        private readonly Dictionary<Type, UnityEngine.Object> _injections;
        private readonly SystemRegister _systemRegister;

        public FieldsInjector(IEnumerable<UnityEngine.Object> injections)
        {
            _injections = new Dictionary<Type, UnityEngine.Object>(injections.Count());
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            foreach(UnityEngine.Object injection in injections)
                _injections.Add(injection.GetType(), injection);

            foreach (SystemModule systemModule in _systemRegister.RegisteredModules)
                InjectFields(systemModule);

            _systemRegister.OnModuleRegister += InjectFields;
            _systemRegister.OnModuleUnregister += TryUnloadInjections;
        }

        private void InjectFields(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.InjectionsFields)
                {
                    if (_injections.ContainsKey(field.FieldType))
                    {
                        field.SetValue(data.System, _injections[field.FieldType]);
                    }
                }
            }
        }

        private void TryUnloadInjections(SystemModule systemModule)
        {

        }
    }
}