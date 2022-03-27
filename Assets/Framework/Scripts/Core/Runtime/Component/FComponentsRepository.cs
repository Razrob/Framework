using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class FComponentsRepository
    {
        private readonly Dictionary<Type, LinkedList<FComponent>> _components;

        public FComponentsRepository()
        {
            _components = new Dictionary<Type, LinkedList<FComponent>>
                (SubTypesFinder.FindTypes(new Type[] { typeof(FComponent) }, Assembly.GetAssembly(typeof(FComponent))).Length);
        }

        public void AddFComponent(FComponent component, Type componentType)
        {
            if (_components.ContainsKey(componentType))
                _components[componentType].AddFirst(component);
            else
            {
                LinkedList<FComponent> list = new LinkedList<FComponent>();
                list.AddFirst(component);
                _components.Add(componentType, list);
            }
        }

        public void RemoveFComponent(FComponent component, Type componentType)
        {
            if (_components.TryGetValue(componentType, out LinkedList<FComponent> list))
                list.Remove(component);
        }

        public IEnumerable<TComponent> GetFComponents<TComponent>() where TComponent : FComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
                return _components[typeof(TComponent)].Select(value => (TComponent)value);

            return new TComponent[0];
        }
    }
}