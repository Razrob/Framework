using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public static class FComponentsFieldsExtractor
    {
        private static readonly BindingFlags FieldsFlags = BindingFlags.Instance | BindingFlags.Public;
        private static readonly BindingFlags MethodsFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        private static readonly Type FComponentType = typeof(FComponent);
        private static readonly string AttachedFEntityFieldName = nameof(FComponent.AttachedEntity);

        public static FieldInfo GetAttachedFEntityFieldInfo()
        {
            return FComponentType.GetField(AttachedFEntityFieldName, FieldsFlags);
        }

        public static IEnumerable<MethodInfo> GetExecutableComponentMethodsInfo()
        {
            string[] methodsNames = Enum.GetNames(typeof(ExecutableComponentMethodID));
            return FComponentType.GetMethods(MethodsFlags).Where(method => methodsNames.Contains(method.Name));
        }

        public static MethodInfo GetExecutableComponentMethodInfo(ExecutableComponentMethodID componentMethodID)
        {
            return FComponentType.GetMethod(componentMethodID.ToString(), MethodsFlags);
        }
    }
}