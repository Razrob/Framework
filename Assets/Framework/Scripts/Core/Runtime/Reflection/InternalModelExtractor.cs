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

        internal static IEnumerable<FieldInfo> GetModelsCollections(Type type)
        {
            IEnumerable<FieldInfo> collectionsFields = type.GetFields(ModelFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectModelAttribute)) != null && 
                field.FieldType.GetInterface(typeof(IEnumerable).FullName) != null && 
                field.FieldType.IsGenericType);

            if (type.BaseType is null)
                return collectionsFields;

            return collectionsFields.Concat(GetModelsCollections(type.BaseType));
        }
    }
}