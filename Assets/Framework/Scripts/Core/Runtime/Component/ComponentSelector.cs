using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class ComponentSelector<TComponent> : IEnumerable<TComponent> where TComponent : FComponent
    {
        public readonly IEnumerable<TComponent> Components;
        public TComponent First => this[0];

        public ComponentSelector(FComponentsRepository componentsRepository)
        {
            Components = componentsRepository.GetFComponents<TComponent>();
        }

        public TComponent this[int index] => Components.ElementAt(index);

        IEnumerator<TComponent> IEnumerable<TComponent>.GetEnumerator() => Components.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();
    }
}