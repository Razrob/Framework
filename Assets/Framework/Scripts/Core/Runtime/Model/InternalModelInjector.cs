using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class InternalModelInjector : IBootLoadElement
    {
        private readonly Dictionary<Type, object> _models;
        private readonly SystemRegister _systemRegister;

        private const string SaveDirectory = "/InternalModel";

        public InternalModelInjector()
        {
            _models = new Dictionary<Type, object>();
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            foreach (SystemModule systemModule in _systemRegister.RegisteredModules)
                InjectModel(systemModule);

            _systemRegister.OnModuleRegister += InjectModel;
            _systemRegister.OnModuleUnregister += TryUnloadModel;
        }

        public void SaveModel()
        {
            foreach(SystemModule module in _systemRegister.RegisteredModules)
            {
                foreach (SystemData data in module.SystemsData)
                {
                    foreach(FieldInfo field in data.InternalModelFields)
                    {
                        InternalModelAttribute modelAttribute = field.GetCustomAttribute<InternalModelAttribute>();

                        if (!modelAttribute.SaveAllow)
                            continue;

                        DataSaver.Save(field.GetValue(data.System), FormFileName(field.FieldType), SaveDirectory);
                    }
                }
            }
        }

        private void InjectModel(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.InternalModelFields)
                {
                    if (_models.ContainsKey(field.FieldType))
                    {
                        field.SetValue(data.System, _models[field.FieldType]);
                    }
                    else
                    {
                        object value = DataLoader.Load(FormFileName(field.FieldType), SaveDirectory);

                        if(value is null)
                            value = Activator.CreateInstance(field.FieldType);

                        _models.Add(field.FieldType, value);
                        field.SetValue(data.System, _models[field.FieldType]);
                    }
                }
            }
        }

        private void TryUnloadModel(SystemModule systemModule)
        {

        }

        private string FormFileName(Type type)
        {
            return $"InternalData_{type.Name}";
        }
    }
}