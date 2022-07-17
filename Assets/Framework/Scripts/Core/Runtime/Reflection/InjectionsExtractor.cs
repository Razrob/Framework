using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class InjectionsExtractor
    {
        private static readonly BindingFlags InjectionFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        internal static IEnumerable<FieldInfo> GetInjectionsData(Type type)
        {
            IEnumerable<FieldInfo> fields = type.GetFields(InjectionFlags)
                .Where(field => field.GetCustomAttribute(typeof(InjectFieldAttribute)) != null);

            if (type.BaseType is null)
                return fields;

            return fields.Concat(GetInjectionsData(type.BaseType));
        }
    }
}