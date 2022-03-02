using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class FrameworkCommander
    {
        public static void SetState(Enum state)
        {
            LoadElementAdapter<StateMachine>.Instance.SetState(state);
        }

        public static void DestroyFEntity(FEntity entity, bool destroyGameObjectAlso = true)
        {
            LoadElementAdapter<FEntityRegister>.Instance.UnregisterFEntity(entity);
            
            if(destroyGameObjectAlso)
                UnityEngine.Object.Destroy(entity.AttachedGameObject);
        }

        public static void DestroyFComponent(FComponent component)
        {
            component.AttachedEntity.RemoveFComponent(component);
        }
    }
}