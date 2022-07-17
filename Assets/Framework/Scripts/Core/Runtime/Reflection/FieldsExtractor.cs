using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class FieldsExtractor
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static readonly Type[] _excludedTypes = { typeof(Action), typeof(Action<>), typeof(void*), typeof(Delegate), typeof(Pointer) };

        internal static IEnumerable<FieldInfo> GetTargetFields(Type type)
        {
            IEnumerable<FieldInfo> fields = type.GetFields(Flags)
                .Where(field => !_excludedTypes.Contains(field.FieldType) && field.FieldType != type &&
                    !field.IsStatic && !field.FieldType.IsPrimitive && field.FieldType.GetInterface(typeof(IEnumerable).FullName) is null);

            if (type.BaseType is null)
                return fields;

            return fields.Concat(GetTargetFields(type.BaseType));
        }
    }
}