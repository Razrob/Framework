using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    [Serializable]
    public class SerializableType
    {
        [SerializeField] private string _typeName;
        public Type Type => Type.GetType(_typeName);
    }
}