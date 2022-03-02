using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class FEntityProvider : MonoBehaviour
    {
        [SerializeReference] private FComponentProvider[] _componentProviders;

        public IReadOnlyList<FComponentProvider> ComponentProviders => _componentProviders;

        private void Awake()
        {
            LoadElementAdapter<FEntityRegister>.Instance.RegisterFEntity(this);
        }
    }
}  