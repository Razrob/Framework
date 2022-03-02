using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FrameworkSystemAttribute : Attribute
    {
        private readonly int[] _attachedStates;
        public IReadOnlyCollection<int> AttachedStates => _attachedStates;

        public readonly Type StateType;

        public FrameworkSystemAttribute(object attachedState)
        {
            if (attachedState is null)
                throw new NullReferenceException("State can not be null. The system will not execute");

            try
            {
                _attachedStates = new int[] { Convert.ToInt32(attachedState) };
            }
            catch(InvalidCastException)
            {
                _attachedStates = null;
                throw new InvalidCastException("Incorrect attached state. The system will not execute");
            }
        }

        public FrameworkSystemAttribute(params object[] attachedStates)
        {
            if (attachedStates is null)
                throw new NullReferenceException("States can not be null. The system will not execute");

            if (attachedStates.Length == 0)
                throw new NullReferenceException("Number of states must be greater than zero. The system will not execute");

            try
            {
                _attachedStates = new int[attachedStates.Length];

                for(int i = 0; i < attachedStates.Length; i++)
                    _attachedStates[i] = Convert.ToInt32(attachedStates[i]);

                _attachedStates.Distinct();
            }
            catch (InvalidCastException)
            {
                _attachedStates = null;
                throw new InvalidCastException("Incorrect attached states. The system will not execute");
            }
        }
    }
}