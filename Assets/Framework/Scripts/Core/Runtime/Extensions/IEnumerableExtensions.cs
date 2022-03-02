using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public static class IEnumerableExtensions
    {
        public static TValue Find<TValue>(this IEnumerable<TValue> enumerable, TValue target)
        {
            if (target is null)
                throw new NullReferenceException("Target can not be null");

            foreach (TValue value in enumerable)
                if (value.Equals(target))
                    return value;

            return default;
        }
    }
}