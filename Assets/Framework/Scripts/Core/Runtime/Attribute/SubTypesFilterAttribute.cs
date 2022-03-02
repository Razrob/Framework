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

        public SubTypesFilterAttribute(Type baseType)
        {
            BaseType = baseType;
            Assembly = Assembly.GetAssembly(baseType);
        }

        public SubTypesFilterAttribute(Type baseType, Type assemblyType)
        {
            BaseType = baseType;
            Assembly = Assembly.GetAssembly(assemblyType);
        }
    }
}