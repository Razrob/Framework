using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class LoadElementAdapter<TLoadElement> where TLoadElement : IBootLoadElement
    {
        private static object _empty = new object();

        private static TLoadElement _instance;
        public static TLoadElement Instance => GetLoadElement();

        public static TLoadElement Initialize(TLoadElement loadElement)
        {
            if (_instance is null)
            {
                lock (_empty)
                {
                    _instance = loadElement;
                }
            }

            return _instance;
        }

        private static TLoadElement GetLoadElement()
        {
            if (_instance is null)
                throw new NullReferenceException("Element not initialized");

            return _instance;
        }
    }
}