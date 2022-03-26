using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

public class EntityBinder : MonoBehaviour, IEntityBinder
{
    public FEntity BindedEntity { get; private set; }

    public void BindEntity(FEntity entity)
    {
        BindedEntity = entity;
    }
}
