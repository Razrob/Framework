using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public static class InternalModelExtractor
    {
        private static readonly BindingFlags ModelFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetInternalModelData(Type systemType)
        {
            IEnumerable<FieldInfo> fields = systemType.GetFields(ModelFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectModelAttribute)) != null 
                && field.FieldType.GetCustomAttribute<InternalModelAttribute>() != null);

            if (systemType.BaseType is null)
                return fields;

            return fields.Concat(GetInternalModelData(systemType.BaseType));
        }
    }
}