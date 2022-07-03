using System;

namespace Framework.Core.Runtime
{
    [Serializable]
    public abstract class FComponent
    {
        public readonly FEntity AttachedEntity;

        protected virtual void OnAttach(FEntity entity) { }
        protected virtual void OnDetach(FEntity entity) { }
    }
} 