using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class InjectionsExtractor
    {
        private static readonly BindingFlags InjectionFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        internal static IEnumerable<FieldInfo> GetInjectionsData(Type systemType, bool withAttributeOnly = true)
        {
            IEnumerable<FieldInfo> fields = systemType.GetFields(InjectionFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectFieldAttribute)) != null || !withAttributeOnly);

            if (systemType.BaseType is null)
                return fields;

            return fields.Concat(GetInjectionsData(systemType.BaseType));
        }
    }
}