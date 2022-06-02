using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    [Serializable]
    public class FComponentProvider
    {
        [SerializeReference] private FComponent _component;
        [SerializeReference] private string _componentType;

        public FComponent Component => _component;
        public Type ComponentType => Type.GetType(_componentType);

        public FComponentProvider(Type componentType, FComponent component)
        {
            _component = component;
            _componentType = componentType.FullName;
        }
    }
}