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
        public readonly Assembly Assembly;
        public readonly bool AllowUndefined;

        public SubTypesFilterAttribute(Type[] baseTypes, bool allowUndefined = false)
        {
            BaseTypes = baseTypes;
            Assembly = Assembly.GetAssembly(baseTypes.ElementAt(0));
            AllowUndefined = allowUndefined;
        }

        public SubTypesFilterAttribute(Type[] baseTypes, Type assemblyType, bool allowUndefined = false)
        {
            BaseTypes = baseTypes;
            Assembly = Assembly.GetAssembly(assemblyType);
            AllowUndefined = allowUndefined;
        }
    }
}