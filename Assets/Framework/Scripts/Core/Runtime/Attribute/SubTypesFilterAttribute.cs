using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SubTypesFilterAttribute : Attribute
    {
        public readonly IEnumerable<Type> BaseTypes;
        public readonly bool AllowUndefined;

        public SubTypesFilterAttribute(Type[] baseTypes, bool allowUndefined = false)
        {
            BaseTypes = baseTypes;
            AllowUndefined = allowUndefined;
        }
    }
}