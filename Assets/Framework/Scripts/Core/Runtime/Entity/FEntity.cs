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

        public FEntity(FEntityProvider entityProvider)
        {
            AttachedGameObject = entityProvider.gameObject;
            _components = new Dictionary<Type, FComponent>(entityProvider.ComponentProviders.Count);
            _entityBinders = new Dictionary<Type, IEntityBinder>(entityProvider.BindersTypes.Count());

            foreach (FComponentProvider componentProvider in entityProvider.ComponentProviders)
                _components.Add(componentProvider.ComponentType, componentProvider.Component);

            foreach (Type binderType in entityProvider.BindersTypes)
            {
                _entityBinders.Add(binderType, (IEntityBinder)AttachedGameObject.AddComponent(binderType));
                _entityBinders[binderType].BindEntity(this);
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

        public TComponent AddFComponent<TComponent>() where TComponent : FComponent, new()
        {
            Type type = typeof(TComponent);
            if (_components.ContainsKey(type))
                return null;

            TComponent component = new TComponent();
            _components.Add(type, component);

            OnFComponentAdd?.Invoke(component);

            return component;
        }

        public bool RemoveFComponent<TComponent>() where TComponent : FComponent
        {
            Type type = typeof(TComponent);
            if (_components.ContainsKey(type))
            {
                FComponent component = _components[type];
                _components.Remove(typeof(TComponent));

                OnFComponentRemove?.Invoke(component);

                return true;
            }

            return false;
        }

        public bool RemoveFComponent(FComponent component)
        {
            if (_components.ContainsValue(component))
            {
                foreach (Type componentType in _components.Keys)
                {
                    if (_components[componentType] != component)
                        continue;

                    _components.Remove(componentType);

                    OnFComponentRemove?.Invoke(component);

                    break;
                }

                return true;
            }

            return false;
        }
    }
}