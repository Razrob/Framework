using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public static class FComponentsFieldsExtractor
    {
        private static readonly BindingFlags FieldsFlags = BindingFlags.Instance | BindingFlags.Public;

        private static readonly Type FComponentType = typeof(FComponent);
        private static readonly string AttachedFEntityFieldName = nameof(FComponent.AttachedEntity);

        public static FieldInfo GetAttachedFEntityFieldInfo()
        {
            return FComponentType.GetField(AttachedFEntityFieldName, FieldsFlags);
        }
    }
}