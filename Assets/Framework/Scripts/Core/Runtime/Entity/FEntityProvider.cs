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
        
        [SubTypesFilter(new Type[] { typeof(IEntityBinder), typeof(MonoBehaviour) }, true)]
        [SerializeField] private SerializableType[] _entityBinders;

        public IReadOnlyList<FComponentProvider> ComponentProviders => _componentProviders;
        public IEnumerable<Type> BindersTypes => _entityBinders.Select(binder => binder.Type);

        private void Awake()
        {
            LoadElementAdapter<FEntityRegister>.Instance.RegisterFEntity(this);
        }
    }
}  