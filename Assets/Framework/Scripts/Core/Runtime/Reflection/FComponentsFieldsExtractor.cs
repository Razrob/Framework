using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal static class FComponentsFieldsExtractor
    {
        private static readonly BindingFlags FieldsFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly BindingFlags MethodsFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        private static readonly Type FComponentType = typeof(FComponent);
        private static readonly string AttachedFEntityFieldName = nameof(FComponent.AttachedEntity);

        internal static FieldInfo GetAttachedFEntityFieldInfo()
        {
            return FComponentType.GetField(AttachedFEntityFieldName, FieldsFlags);
        }

        internal static IEnumerable<MethodInfo> GetExecutableComponentMethodsInfo()
        {
            string[] methodsNames = Enum.GetNames(typeof(ExecutableComponentMethodID));
            return FComponentType.GetMethods(MethodsFlags).Where(method => methodsNames.Contains(method.Name));
        }

        internal static MethodInfo GetExecutableComponentMethodInfo(ExecutableComponentMethodID componentMethodID)
        {
            return FComponentType.GetMethod(componentMethodID.ToString(), MethodsFlags);
        }
    }
}