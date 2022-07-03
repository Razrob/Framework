using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class ComponentSelectorExtractor
    {
        private static readonly BindingFlags SelectorsFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static readonly Type SelectorType = typeof(ComponentSelector<>);

        internal static IEnumerable<FieldInfo> GetSelectors(Type systemType)
        {
            return systemType.GetFields(SelectorsFlags)
                .Where(field => field.FieldType.IsGenericType)
                .Where(field => field.FieldType.GetGenericTypeDefinition() == SelectorType)
                .Where(field => field.GetCustomAttribute<InjectSelectorAttribute>() != null);
        }
    }
}