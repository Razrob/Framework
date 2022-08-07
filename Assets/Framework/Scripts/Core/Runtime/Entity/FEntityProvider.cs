using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class FEntityProvider : MonoBehaviour
    {
        [SerializeField] private InitializationMethod _initializationMethod;
        [SerializeReference] private FComponentProvider[] _componentProviders;
        
        [SubTypesFilter(new Type[] { typeof(IEntityBinder), typeof(MonoBehaviour) }, true)]
        [SerializeField] private SerializableType[] _entityBinders;

        private bool _initialized;

        internal IReadOnlyList<FComponentProvider> ComponentProviders => _componentProviders;
        internal IEnumerable<Type> BindersTypes => _entityBinders.Select(binder => binder.Type);
        public readonly FEntity InstantiatedEntity;

        private void Awake() => Register(InitializationMethod.Awake);
        private void OnEnable() => Register(InitializationMethod.OnEnable); 
        private void Start() => Register(InitializationMethod.Start);

        private void Register(InitializationMethod calledMethod)
        {
            if (calledMethod != _initializationMethod || _initialized)
                return;

            _initialized = true;

            LoadElementAdapter<FEntityRegister>.Instance.RegisterFEntity(this);
        }
    }
}  