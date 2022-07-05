using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    [Serializable]
    public abstract class InternalModel : ISerializationCallbackReceiver
    {
        protected virtual void OnInject() { }

        public virtual void OnAfterDeserialize() { }
        public virtual void OnBeforeSerialize() { }
    }
}