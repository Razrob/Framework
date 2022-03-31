using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public static class InjectionsExtractor
    {
        private static readonly BindingFlags InjectionFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetInjectionsData(Type systemType)
        {
            IEnumerable<FieldInfo> fields = systemType.GetFields(InjectionFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectFieldAttribute)) != null);

            if (systemType.BaseType is null)
                return fields;

            return fields.Concat(GetInjectionsData(systemType.BaseType));
        }
    }
}