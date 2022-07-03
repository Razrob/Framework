using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class FEntityProvider : MonoBehaviour
    {
        [SerializeReference] private FComponentProvider[] _componentProviders;
        
        [SubTypesFilter(new Type[] { typeof(IEntityBinder), typeof(MonoBehaviour) }, true)]
        [SerializeField] private SerializableType[] _entityBinders;

        internal IReadOnlyList<FComponentProvider> ComponentProviders => _componentProviders;
        internal IEnumerable<Type> BindersTypes => _entityBinders.Select(binder => binder.Type);
        internal readonly FEntity InstantiatedEntity;

        private void Awake()
        {
            LoadElementAdapter<FEntityRegister>.Instance.RegisterFEntity(this);
        }
    }
}  