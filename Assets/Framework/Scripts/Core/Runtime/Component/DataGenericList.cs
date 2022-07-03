using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal class DataGenericList<TSub, TBase> : DataList<TBase> where TSub : TBase
    {
        private readonly LinkedList<TSub> _list;
        internal IReadOnlyCollection<TSub> Elements => _list;

        internal DataGenericList()
        {
            _list = new LinkedList<TSub>();
        }

        internal override void AddElementBase(TBase value) => AddElementSub((TSub)value);
        internal override void RemoveElementBase(TBase value) => RemoveElementSub((TSub)value);

        internal void AddElementSub(TSub value)
        {
            _list.AddLast(value);
        }

        internal void RemoveElementSub(TSub value)
        {
            _list.Remove(value);
        }
    }
}