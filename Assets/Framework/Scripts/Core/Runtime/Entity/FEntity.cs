using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class FEntity
    {
        private readonly Dictionary<Type, FComponent> _components;
        private readonly Dictionary<Type, IEntityBinder> _entityBinders;

        public IReadOnlyDictionary<Type, FComponent> FComponents => _components;
        public IReadOnlyDictionary<Type, IEntityBinder> EntityBinders => _entityBinders;

        public readonly GameObject AttachedGameObject;

        public event FrameworkDelegate<FComponent> OnFComponentAdd;
        public event FrameworkDelegate<FComponent> OnFComponentRemove;

        internal FEntity(FEntityProvider entityProvider)
        {
            AttachedGameObject = entityProvider.gameObject;
            _components = new Dictionary<Type, FComponent>(entityProvider.ComponentProviders.Count);
            _entityBinders = new Dictionary<Type, IEntityBinder>(entityProvider.BindersTypes.Count());

            FComponentsRepository componentsRepository = LoadElementAdapter<FComponentsRepository>.Instance;

            OnFComponentAdd += componentsRepository.AddFComponent;
            OnFComponentRemove += componentsRepository.RemoveFComponent;

            foreach (FComponentProvider componentProvider in entityProvider.ComponentProviders)
            {
                Type baseComponentType = componentProvider.ComponentType;

                while (baseComponentType != typeof(FComponent))
                {
                    _components.Add(baseComponentType, componentProvider.Component);
                    baseComponentType = baseComponentType.BaseType;
                }

                OnFComponentAdd?.Invoke(componentProvider.Component);
            }

            foreach (Type binderType in entityProvider.BindersTypes)
            {
                Type baseBinderType = binderType;
                IEntityBinder binder = (IEntityBinder)AttachedGameObject.AddComponent(binderType);

                while (baseBinderType != typeof(MonoBehaviour) && baseBinderType != typeof(object) && baseBinderType != null)
                {
                    _entityBinders.Add(baseBinderType, binder);
                    _entityBinders[baseBinderType].BindEntity(this);
                    baseBinderType = baseBinderType.BaseType;
                }
            }
        }

        public TBinder GetBinder<TBinder>() where TBinder : MonoBehaviour, IEntityBinder
        {
            Type type = typeof(TBinder);
            if (_entityBinders.TryGetValue(type, out IEntityBinder entityBinder))
                return entityBinder as TBinder;

            return null;
        }

        public TComponent GetFComponent<TComponent>() where TComponent : FComponent
        {
            Type type = typeof(TComponent);
            if (_components.TryGetValue(type, out FComponent component))
                return component as TComponent;

            return null;
        }

        public void AddFComponent(FComponent component)
        {
            if (component is null)
                return;

            Type type = component.GetType();
            if (_components.ContainsKey(type))
                return;

            while (type != typeof(FComponent))
            {
                _components.Add(type, component);
                type = type.BaseType;
            }

            OnFComponentAdd?.Invoke(component);
        }

        public bool RemoveFComponent<TComponent>() where TComponent : FComponent
        {
            Type type = typeof(TComponent);
            bool removed = false;
            FComponent component = null;

            while (type != typeof(FComponent))
            {
                if (_components.ContainsKey(type))
                {
                    component = _components[type];
                    _components.Remove(typeof(TComponent));
                    removed = true;
                }

                type = type.BaseType;
            }

            if (removed)
                OnFComponentRemove?.Invoke(component);

            return removed;
        }

        public bool RemoveFComponent(FComponent component)
        {
            Type type = component.GetType();
            bool removed = false;

            while (type != typeof(FComponent))
            {
                if (_components.ContainsKey(type))
                {
                    _components.Remove(type);
                    removed = true;
                }
            }

            if (removed)
                OnFComponentRemove?.Invoke(component);

            return removed;
        }
    }
}