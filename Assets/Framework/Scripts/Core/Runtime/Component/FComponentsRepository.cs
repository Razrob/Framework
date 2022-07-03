using System.Collections.Generic;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class FComponentsRepository : IBootLoadElement
    {
        private readonly Dictionary<Type, LinkedList<FComponent>> _components;

        internal FComponentsRepository()
        {
            _components = new Dictionary<Type, LinkedList<FComponent>>();
        }

        internal void AddFComponent(FComponent component)
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

        internal void RemoveFComponent(FComponent component)
        {
            Type componentBaseType = component.GetType();

            while (componentBaseType != typeof(FComponent))
            {
                if (_components.TryGetValue(componentBaseType, out LinkedList<FComponent> list))
                    list.Remove(component);

                componentBaseType = componentBaseType.BaseType;
            }
        }

        internal IEnumerable<TComponent> GetFComponents<TComponent>() where TComponent : FComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
                return _components[typeof(TComponent)].Select(value => (TComponent)value);

            return new TComponent[0];
        }

        public void OnBootLoadComplete() { }
    }
}