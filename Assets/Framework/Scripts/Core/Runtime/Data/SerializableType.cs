using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    [Serializable]
    public class SerializableType
    {
        [SerializeField] private string _typeName;
        [SerializeField] private string _assemblyName;
        public Type Type => Assembly.Load(_assemblyName).GetType(_typeName);
    }
}