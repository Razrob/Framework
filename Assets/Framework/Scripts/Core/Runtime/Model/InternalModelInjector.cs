using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal class InternalModelInjector : IBootLoadElement
    {
        private readonly Dictionary<Type, InternalModel> _models;
        private readonly SystemRegister _systemRegister;

        private readonly MethodInfo _onInjectMethodInfo;

        private const string SaveDirectory = "InternalModel";

        internal IReadOnlyDictionary<Type, InternalModel> Models => _models;

        internal event FrameworkDelegate<InternalModel> OnModelCreate;

        internal InternalModelInjector()
        {
            _models = new Dictionary<Type, InternalModel>();
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            _onInjectMethodInfo = InternalModelExtractor.GetOnInjectMethodInfo();

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
            if (!ModelTypeIsValid(modelField.FieldType))
                return;

            if (_models.ContainsKey(modelField.FieldType))
            {
                modelField.SetValue(declaredObject, _models[modelField.FieldType]);
            }
            else
            {
                InternalModel value = DataLoader.Load(FormFileName(modelField.FieldType), modelField.FieldType) as InternalModel;

                if (value is null)
                    value = Activator.CreateInstance(modelField.FieldType) as InternalModel;

                _models.Add(modelField.FieldType, value);
                modelField.SetValue(declaredObject, _models[modelField.FieldType]);

                OnModelCreate?.Invoke(value);
            }
        }

        private bool ModelTypeIsValid(Type modelType)
        {
            return modelType.IsSubclassOf(typeof(InternalModel));
        }

        private void TryUnloadModel(SystemModule systemModule)
        {

        }

        private string FormFileName(Type type)
        {
            return $"{SaveDirectory}/InternalData_{type.Name}";
        }

        internal void SaveModel(Type targetModelType = null)
        {
            HashSet<Type> savedTypes = new HashSet<Type>();

            foreach (SystemModule module in _systemRegister.RegisteredModules)
            {
                foreach (SystemData data in module.SystemsData)
                {
                    foreach (FieldInfo field in data.ModelInjectionsFields)
                    {
                        InternalModelAttribute modelAttribute = field.GetCustomAttribute<InternalModelAttribute>();

                        if (targetModelType != null && field.FieldType != targetModelType)
                            continue;

                        if (!modelAttribute.SaveAllow || savedTypes.Contains(field.FieldType))
                            continue;

                        savedTypes.Add(field.FieldType);
                        DataSaver.Save(field.GetValue(data.System), FormFileName(field.FieldType));
                    }
                }
            }
        }

        public void OnBootLoadComplete() 
        {
            foreach(InternalModel model in _models.Values)
                _onInjectMethodInfo.Invoke(model, null);
        }
    }
}