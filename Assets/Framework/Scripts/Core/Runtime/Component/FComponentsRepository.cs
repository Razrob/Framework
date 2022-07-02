using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class FComponentsRepository : IBootLoadElement
    {
        private readonly Dictionary<Type, LinkedList<FComponent>> _components;

        public FComponentsRepository()
        {
            _components = new Dictionary<Type, LinkedList<FComponent>>();
        }

        public void AddFComponent(FComponent component)
        {
            Type componentBaseType = component.GetType();

            while (componentBaseType != typeof(FComponent))
            {
                if (_components.ContainsKey(componentBaseType))
                    _components[componentBaseType].AddFirst(component);
                else
                {
                    LinkedList<FComponent> list = new LinkedList<FComponent>();
                    list.AddFirst(component);
                    _components.Add(componentBaseType, list);
                }

                componentBaseType = componentBaseType.BaseType;
            }
        }

        public void RemoveFComponent(FComponent component)
        {
            Type componentBaseType = component.GetType();

            while (componentBaseType != typeof(FComponent))
            {
                if (_components.TryGetValue(componentBaseType, out LinkedList<FComponent> list))
                    list.Remove(component);

                componentBaseType = componentBaseType.BaseType;
            }
        }

        public IEnumerable<TComponent> GetFComponents<TComponent>() where TComponent : FComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
                return _components[typeof(TComponent)].Select(value => (TComponent)value);

            return new TComponent[0];
        }

        public void OnBootLoadComplete() { }
    }
}