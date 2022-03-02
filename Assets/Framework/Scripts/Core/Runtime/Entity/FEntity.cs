using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class FEntity
    {
        private readonly Dictionary<Type, FComponent> _components;
        public IReadOnlyDictionary<Type, FComponent> FComponents => _components;

        public readonly GameObject AttachedGameObject;

        public event FrameworkEvent<FComponent, Type> OnFComponentAdd;
        public event FrameworkEvent<FComponent, Type> OnFComponentRemove;

        public FEntity(IReadOnlyList<FComponentProvider> componentProviders, GameObject attachedGameObject)
        {
            AttachedGameObject = attachedGameObject;
            _components = new Dictionary<Type, FComponent>(componentProviders.Count);

            foreach (FComponentProvider componentProvider in componentProviders)
                _components.Add(componentProvider.ComponentType, componentProvider.Component);
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

            OnFComponentAdd?.Invoke(component, type);

            return component;
        }

        public bool RemoveFComponent<TComponent>() where TComponent : FComponent
        {
            Type type = typeof(TComponent);
            if (_components.ContainsKey(type))
            {
                FComponent component = _components[type];
                _components.Remove(typeof(TComponent));

                OnFComponentRemove?.Invoke(component, type);

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

                    OnFComponentRemove?.Invoke(component, componentType);

                    break;
                }

                return true;
            }

            return false;
        }
    }
}