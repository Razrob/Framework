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

        private void InjectModel(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.ModelInjectionsFields)
                {
                    ScanAndInjectInternalModelTypes(field, data.System);
                }
            }
        }

        private void ScanAndInjectInternalModelTypes(FieldInfo field, object declaredOnject)
        {
            InjectModel(field, declaredOnject);

            foreach (FieldInfo sytemExcludedModelField in InternalModelExtractor.GetInternalModelData(field.FieldType))
            { 
                InjectModel(sytemExcludedModelField, _models[field.FieldType]);
                ScanAndInjectInternalModelTypes(sytemExcludedModelField, _models[field.FieldType]);
            }
        }

        private void InjectModel(FieldInfo modelField, object declaredObject)
        {
            if (_models.ContainsKey(modelField.FieldType))
            {
                modelField.SetValue(declaredObject, _models[modelField.FieldType]);
            }
            else
            {
                object value = DataLoader.Load(FormFileName(modelField.FieldType), SaveDirectory);

                if (value is null)
                    value = Activator.CreateInstance(modelField.FieldType);

                _models.Add(modelField.FieldType, value);
                modelField.SetValue(declaredObject, _models[modelField.FieldType]);
            }
        }

        private void TryUnloadModel(SystemModule systemModule)
        {

        }

        private string FormFileName(Type type)
        {
            return $"InternalData_{type.Name}";
        }

        public void SaveModel()
        {
            Dictionary<Type, object> savedTypes = new Dictionary<Type, object>();

            foreach (SystemModule module in _systemRegister.RegisteredModules)
            {
                foreach (SystemData data in module.SystemsData)
                {
                    foreach (FieldInfo field in data.ModelInjectionsFields)
                    {
                        InternalModelAttribute modelAttribute = field.GetCustomAttribute<InternalModelAttribute>();

                        if (!modelAttribute.SaveAllow || savedTypes.ContainsKey(field.FieldType))
                            continue;

                        savedTypes.Add(field.FieldType, new object());
                        DataSaver.Save(field.GetValue(data.System), FormFileName(field.FieldType), SaveDirectory);
                    }
                }
            }
        }
    }
}