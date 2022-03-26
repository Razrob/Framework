using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public interface IEntityBinder
    {
        public FEntity BindedEntity { get; }
        public void BindEntity(FEntity entity);
    }
}