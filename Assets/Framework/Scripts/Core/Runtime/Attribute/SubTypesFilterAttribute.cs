using System.Collections.Generic;
using System;
namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SubTypesFilterAttribute : Attribute
    {
        internal readonly IEnumerable<Type> BaseTypes;
        internal readonly bool AllowUndefined;

        public SubTypesFilterAttribute(Type[] baseTypes, bool allowUndefined = false)
        {
            BaseTypes = baseTypes;
            AllowUndefined = allowUndefined;
        }
    }
}