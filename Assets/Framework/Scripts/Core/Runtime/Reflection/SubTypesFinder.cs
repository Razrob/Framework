using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework.Core.Runtime
{
    public class SubTypesFinder
    {
        public IReadOnlyList<Type> SubTypes { get; private set; }

        private Type _baseType;

        public SubTypesFinder(Type baseType)
        {
            _baseType = baseType;
            SubTypes = FindTypes(_baseType.Assembly, baseType);
        }

        public SubTypesFinder(Type baseType, Assembly targetAssembly)
        {
            _baseType = baseType;
            SubTypes = FindTypes(targetAssembly, baseType);
        }

        public static Type[] FindTypes(Assembly assembly, Type baseType)
        {
            if (baseType is null)
                throw new NullReferenceException();

            if(baseType.IsClass)
                return assembly.GetTypes().Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract).ToArray();
            else
                return assembly.GetTypes().Where(type => type.GetInterface(baseType.ToString()) != null && !type.IsAbstract).ToArray();
        }
    }
}