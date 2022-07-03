using System.Collections.Generic;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FrameworkSystemAttribute : Attribute
    {
        private readonly int[] _attachedStates;
        internal IReadOnlyCollection<int> AttachedStates => _attachedStates;

        internal readonly Type StateType;

        public FrameworkSystemAttribute(object attachedState)
        {
            if (attachedState is null)
                FrameworkDebuger.Log(Runtime.LogType.Exception, "State can not be null. The system will not execute");

            try
            {
                _attachedStates = new int[] { Convert.ToInt32(attachedState) };
            }
            catch(InvalidCastException)
            {
                _attachedStates = null;
                FrameworkDebuger.Log(Runtime.LogType.Exception, "Incorrect attached state. The system will not execute");
            }
        }

        public FrameworkSystemAttribute(params object[] attachedStates)
        {
            if (attachedStates is null)
                FrameworkDebuger.Log(Runtime.LogType.Exception, "States can not be null. The system will not execute");

            if (attachedStates.Length == 0)
                FrameworkDebuger.Log(Runtime.LogType.Exception, "Number of states must be greater than zero. The system will not execute");

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
                FrameworkDebuger.Log(Runtime.LogType.Exception, "Incorrect attached states. The system will not execute");
            }
        }
    }
}