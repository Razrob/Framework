using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    [Serializable]
    public class FComponentProvider
    {
        [SerializeReference] private FComponent _component;
        [SerializeReference] private string _componentType;
        [SerializeReference] private string _componentTypeAssemblyName;

        public FComponent Component => _component;
        public Type ComponentType => Assembly.Load(_componentTypeAssemblyName).GetType(_componentType);

        public FComponentProvider(Type componentType, Assembly typeAssembly, FComponent component)
        {
            _component = component;
            _componentType = componentType.FullName;
            _componentTypeAssemblyName = typeAssembly.FullName;
        }
    }
}