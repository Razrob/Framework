using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class InternalModelExtractor
    {
        private static readonly BindingFlags ModelFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static readonly Type InternalModelType = typeof(InternalModel);
        private const string OnInjectMethodName = "OnInject";
        
        private static readonly Type[] ExcludedTypes = { typeof(Action), typeof(Action<>), typeof(void*), typeof(Delegate), typeof(Pointer), 
            typeof(Type) };

        internal static IEnumerable<FieldInfo> GetInternalModelData(Type systemType)
        {
            IEnumerable<FieldInfo> fields = systemType.GetFields(ModelFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectModelAttribute)) != null 
                && field.FieldType.GetCustomAttribute<InternalModelAttribute>() != null);

            if (systemType.BaseType is null)
                return fields;

            return fields.Concat(GetInternalModelData(systemType.BaseType));
        }

        internal static MethodInfo GetOnInjectMethodInfo()
        {
            return InternalModelType.GetMethod(OnInjectMethodName, ModelFlags);
        }

        internal static IEnumerable<InternalModel> GetAllModels(IEnumerable<InternalModel> singleModels)
        {
            HashSet<InternalModel> models = new HashSet<InternalModel>();
            HashSet<Type> excludedTypes = new HashSet<Type>(ExcludedTypes);

            foreach (InternalModel singleModel in singleModels)
                GetAllModelsInsideModel(singleModel, ref models, ref excludedTypes);

            return models;
        }

        private static void GetAllModelsInsideModel(object @object, ref HashSet<InternalModel> models, ref HashSet<Type> excludedTypes)
        {
            Type type = @object.GetType();

            //if (!excludedTypes.Add(type))
            //    return;

            if (type.IsSubclassOf(typeof(InternalModel)))
                if (!models.Add((InternalModel)@object))
                    return;

            foreach (FieldInfo field in type.GetFields(ModelFlags))
            {
                if (ExcludedTypes.Contains(field.FieldType) || field.FieldType == type || field.FieldType.IsPrimitive)
                    continue;

                object fieldObject = field.GetValue(@object);

                if (fieldObject is null)
                    continue;

                IEnumerable collection = fieldObject as IEnumerable;

                if(collection is null)
                {
                    GetAllModelsInsideModel(fieldObject, ref models, ref excludedTypes);
                }
                else
                {
                    foreach (object element in collection)
                    {
                        if (ExcludedTypes.Contains(element.GetType()) || element.GetType() == type || element.GetType().IsPrimitive)
                            continue; 

                        GetAllModelsInsideModel(element, ref models, ref excludedTypes);
                    }
                }
            }
        }
    }
}