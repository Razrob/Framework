using System;

namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecutableAttribute : Attribute
    {
        internal readonly int ExecutionOrder = 0;
        internal readonly bool Execute = true;
        internal readonly bool StateDependency = true;

        public ExecutableAttribute() { }
        public ExecutableAttribute(int executionOrder) => ExecutionOrder = executionOrder;
        public ExecutableAttribute(bool execute) => Execute = execute;
        public ExecutableAttribute(int executionOrder, bool stateDependency)
        {
            ExecutionOrder = executionOrder;
            StateDependency = stateDependency;
        }
    }
}