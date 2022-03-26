using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime 
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SubTypesFilterAttribute : Attribute
    {
        public readonly Type BaseType;
        public readonly Assembly Assembly;
        public readonly bool AllowUndefined;

        public SubTypesFilterAttribute(Type baseType, bool allowUndefined = false)
        {
            BaseType = baseType;
            Assembly = Assembly.GetAssembly(baseType);
            AllowUndefined = allowUndefined;
        }

        public SubTypesFilterAttribute(Type baseType, Type assemblyType, bool allowUndefined = false)
        {
            BaseType = baseType;
            Assembly = Assembly.GetAssembly(assemblyType);
            AllowUndefined = allowUndefined;
        }
    }
}