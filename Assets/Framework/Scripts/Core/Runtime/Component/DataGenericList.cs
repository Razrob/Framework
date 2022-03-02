using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class DataGenericList<TSub, TBase> : DataList<TBase> where TSub : TBase
    {
        private readonly LinkedList<TSub> _list;
        public IReadOnlyCollection<TSub> Elements => _list;

        public DataGenericList()
        {
            _list = new LinkedList<TSub>();
        }

        public override void AddElementBase(TBase value) => AddElementSub((TSub)value);
        public override void RemoveElementBase(TBase value) => RemoveElementSub((TSub)value);

        public void AddElementSub(TSub value)
        {
            _list.AddLast(value);
        }

        public void RemoveElementSub(TSub value)
        {
            _list.Remove(value);
        }
    }
}