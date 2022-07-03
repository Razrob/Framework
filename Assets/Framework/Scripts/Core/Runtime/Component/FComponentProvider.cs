using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    [Serializable]
    internal class FComponentProvider
    {
        [SerializeReference] private FComponent _component;
        [SerializeReference] private string _componentType;
        [SerializeReference] private string _componentTypeAssemblyName;

        internal FComponent Component => _component;
        internal Type ComponentType => Assembly.Load(_componentTypeAssemblyName).GetType(_componentType);

        internal FComponentProvider(Type componentType, Assembly typeAssembly, FComponent component)
        {
            _component = component;
            _componentType = componentType.FullName;
            _componentTypeAssemblyName = typeAssembly.FullName;
        }
    }
}