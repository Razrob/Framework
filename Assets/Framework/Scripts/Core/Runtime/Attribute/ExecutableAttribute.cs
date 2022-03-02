using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecutableAttribute : Attribute
    {
        public readonly int ExecutionOrder = 0;
        public readonly bool Execute = true;
        public readonly bool StateDependency = true;

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