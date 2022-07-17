using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

namespace Framework.Core.Runtime
{
    internal class InternalModelInjector : IBootLoadElement
    {
        private readonly Dictionary<Type, InternalModel> _singleModels;
        private readonly HashSet<InternalModel> _allModels;
        private readonly SystemRegister _systemRegister;

        private readonly MethodInfo _onInjectMethodInfo;

        private const string SaveDirectory = "InternalModel";

        internal IReadOnlyDictionary<Type, InternalModel> SingleModels => _singleModels;
        internal IEnumerable<InternalModel> AllModels => _allModels;

        internal event FrameworkDelegate<InternalModel> OnModelCreate;

        internal InternalModelInjector(IReadOnlyList<Type> modelsToInitializeIntermediate = null)
        {
            _singleModels = new Dictionary<Type, InternalModel>();
            _allModels = new HashSet<InternalModel>();
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            _onInjectMethodInfo = InternalModelExtractor.GetOnInjectMethodInfo();

            if(modelsToInitializeIntermediate != null)
                CreateModelIntermediate(modelsToInitializeIntermediate);

            foreach (SystemModule systemModule in _systemRegister.RegisteredModules)
                InjectModel(systemModule);

            _systemRegister.OnModuleRegister += InjectModel;
            _systemRegister.OnModuleUnregister += TryUnloadModel;
        }

        private void CreateModelIntermediate(IReadOnlyList<Type> modelsToInitializeIntermediate)
        {
            foreach (Type modelType in modelsToInitializeIntermediate)
            {
                InternalModel value = DataLoader.Load(FormFileName(modelType), modelType) as InternalModel;

                if (value is null)
                    value = (InternalModel)Activator.CreateInstance(modelType);

                _singleModels.Add(modelType, value);

                foreach (InternalModel model in InternalModelExtractor.GetAllModels(new InternalModel[] { value }))
                    _allModels.Add(model);
            }
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
                ScanAndInjectInternalModelTypes(sytemExcludedModelField, _singleModels[field.FieldType]);
        }

        private void InjectModel(FieldInfo modelField, object declaredObject)
        {
            if (!ModelTypeIsValid(modelField.FieldType))
                return;

            if (_singleModels.ContainsKey(modelField.FieldType))
            {
                modelField.SetValue(declaredObject, _singleModels[modelField.FieldType]);
            }
            else
            {
                InternalModel value = DataLoader.Load(FormFileName(modelField.FieldType), modelField.FieldType) as InternalModel;

                if (value is null)
                    value = Activator.CreateInstance(modelField.FieldType) as InternalModel;

                _singleModels.Add(modelField.FieldType, value);

                foreach (InternalModel model in InternalModelExtractor.GetAllModels(new InternalModel[] { value }))
                    _allModels.Add(model);

                modelField.SetValue(declaredObject, _singleModels[modelField.FieldType]);
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
                        if (targetModelType != null && field.FieldType != targetModelType)
                            return;

                        ScanAndSaveModelTypes(field, _singleModels[field.FieldType], ref savedTypes);
                    }
                }
            }
        }

        private void ScanAndSaveModelTypes(FieldInfo fieldInfo, object @object, ref HashSet<Type> savedTypes)
        {
            SaveModelType(fieldInfo, @object, ref savedTypes);
            
            foreach (FieldInfo sytemExcludedModelField in InternalModelExtractor.GetInternalModelData(fieldInfo.FieldType))
                ScanAndSaveModelTypes(sytemExcludedModelField, _singleModels[sytemExcludedModelField.FieldType], ref savedTypes);
        }

        private void SaveModelType(FieldInfo fieldInfo, object declaredObject, ref HashSet<Type> savedTypes)
        {
            InternalModelAttribute modelAttribute = fieldInfo.FieldType.GetCustomAttribute<InternalModelAttribute>();

            if (!modelAttribute.SaveAllow || savedTypes.Contains(fieldInfo.FieldType))
                return;

            savedTypes.Add(fieldInfo.FieldType);
            DataSaver.Save(declaredObject, FormFileName(fieldInfo.FieldType));
        }

        public void OnBootLoadComplete() 
        {
            foreach (InternalModel model in _allModels)
                _onInjectMethodInfo.Invoke(model, null);
        }
    }
}