using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class FrameworkCommander
    {
        public static T GetCurrentState<T>() where T : struct, Enum
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), LoadElementAdapter<StateMachine>.Instance.CurrentState);
            }
            catch (InvalidCastException)
            {
                return (T)FrameworkDebuger.Log(LogType.Exception, 
                    "[InvalidCastException], FrameworkCommander.GetCurrentState, generic parameter is not a state");
            }
        }

        public static void SetState(Enum state)
        {
            LoadElementAdapter<StateMachine>.Instance.SetState(state);
        }

        public static void DestroyFEntity(FEntity entity, bool destroyGameObjectAlso = true)
        {
            LoadElementAdapter<FEntityRegister>.Instance.UnregisterFEntity(entity);

            foreach (IEntityBinder entityBinder in entity.EntityBinders.Values)
                UnityEngine.Object.Destroy((Component)entityBinder);

            if (destroyGameObjectAlso)
                UnityEngine.Object.Destroy(entity.AttachedGameObject);
        }

        public static void DestroyFComponent(FComponent component)
        {
            component.AttachedEntity.RemoveFComponent(component);
            LoadElementAdapter<FComponentsRepository>.Instance.RemoveFComponent(component);
        }

        public static void SaveModel()
        {
            LoadElementAdapter<InternalModelInjector>.Instance.SaveModel();
        }

        public static void SaveModel<TModel>() where TModel : InternalModel
        {
            LoadElementAdapter<InternalModelInjector>.Instance.SaveModel(typeof(TModel));
        }
    }
}