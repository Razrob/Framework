using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

namespace Framework.Core.Runtime
{
    [Serializable]
    public class MovementComponent : FComponent
    {
        [SerializeField] public Vector3 CurrentVelocity;
        [SerializeField] public Vector3 CurrentAcceleration;
        [SerializeField] public Transform Transform;

        protected override void OnAttach(FEntity entity)
        {

        }
    }
}