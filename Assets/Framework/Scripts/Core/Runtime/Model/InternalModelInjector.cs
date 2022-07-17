using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

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

        internal InternalModelInjector(IReadOnlyList<Type> modelsToInitializeIntermediate = null)
        {
            _models = new Dictionary<Type, InternalModel>();
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

                _models.Add(modelType, value);
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
                ScanAndInjectInternalModelTypes(sytemExcludedModelField, _models[field.FieldType]);
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
                        if (targetModelType != null && field.FieldType != targetModelType)
                            return;

                        ScanAndSaveModelTypes(field, _models[field.FieldType], ref savedTypes);
                    }
                }
            }
        }

        private void ScanAndSaveModelTypes(FieldInfo fieldInfo, object @object, ref HashSet<Type> savedTypes)
        {
            SaveModelType(fieldInfo, @object, ref savedTypes);
            
            foreach (FieldInfo sytemExcludedModelField in InternalModelExtractor.GetInternalModelData(fieldInfo.FieldType))
                ScanAndSaveModelTypes(sytemExcludedModelField, _models[sytemExcludedModelField.FieldType], ref savedTypes);
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
            foreach (InternalModel model in _models.Values)
            {
                _onInjectMethodInfo.Invoke(model, null);
                ScanAndTryCallInjectMethod(model);
            }
        }

        private void ScanAndTryCallInjectMethod(object @object)
        {
            if (@object is null)
                return;

            Type type = @object.GetType();
            IEnumerable<FieldInfo> modelsCollectionsFields = InternalModelExtractor.GetModelsCollections(type);

            if (type.IsSubclassOf(typeof(InternalModel)) && !_models.ContainsKey(type))
                _onInjectMethodInfo.Invoke(@object, null);

            foreach (FieldInfo field in FieldsExtractor.GetTargetFields(type))
                ScanAndTryCallInjectMethod(field.GetValue(@object));

            foreach (FieldInfo modelsCollectionsField in modelsCollectionsFields)
            {
                IEnumerable collection = modelsCollectionsField.GetValue(@object) as IEnumerable;

                if (collection is null)
                    continue;

                foreach (object element in collection)
                    ScanAndTryCallInjectMethod(element);
            }
        }
    }
}