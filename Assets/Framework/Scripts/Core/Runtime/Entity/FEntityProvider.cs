using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class FEntityProvider : MonoBehaviour
    {
        [SerializeReference] private FComponentProvider[] _componentProviders;
        [SerializeField] [SubTypesFilter(typeof(IEntityBinder), true)] private SerializableType[] _entityBinders;

        public IReadOnlyList<FComponentProvider> ComponentProviders => _componentProviders;
        public IEnumerable<Type> BinderTypes => _entityBinders.Select(binder => binder.Type);

        private void Awake()
        {
            LoadElementAdapter<FEntityRegister>.Instance.RegisterFEntity(this);
        }
    }
}  