using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public abstract class DataList<TBase>
    {
        public abstract void AddElementBase(TBase value);
        public abstract void RemoveElementBase(TBase value);
    }
}