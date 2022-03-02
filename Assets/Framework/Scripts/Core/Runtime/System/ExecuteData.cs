using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Runtime
{
    public class ExecuteData
    {
        private readonly LinkedList<ExecuteDelegate> _invocationList;

        public readonly int ExecuteOrder;
        public ExecuteDelegate ExecuteDelegate;
        public IEnumerable<ExecuteDelegate> InvocationList => _invocationList;

        public ExecuteData(ExecuteDelegate @delegate, int executeOrder)
        {
            _invocationList = new LinkedList<ExecuteDelegate>();
            ExecuteOrder = executeOrder;

            ExecuteDelegate += @delegate;
            _invocationList.AddLast(@delegate);
        }

        public void AddDelegate(ExecuteDelegate @delegate)
        {
            if (!_invocationList.Contains(@delegate))
            {
                ExecuteDelegate += @delegate;
                _invocationList.AddLast(@delegate);
            }
        }

        public bool RemoveDelegate(ExecuteDelegate @delegate)
        {
            ExecuteDelegate -= @delegate;
            return _invocationList.Remove(@delegate);
        }

        public void RemoveAllDelegates()
        {
            ExecuteDelegate = null;
            _invocationList.Clear();
        }

        public bool RemoveTarget(object target)
        {
            LinkedListNode<ExecuteDelegate> node = _invocationList.First;

            while (node != null)
            {
                if (node.Value.Target.Equals(target))
                {
                    ExecuteDelegate -= node.Value;
                    _invocationList.Remove(node);
                    return true;
                }

                node = node.Next;
            }

            return false;
        }
    }

    public class ExecuteData<T>
    {
        private readonly LinkedList<ExecuteDelegate<T>> _invocationList;

        public readonly int ExecuteOrder;
        public ExecuteDelegate<T> ExecuteDelegate;
        public IEnumerable<ExecuteDelegate<T>> InvocationList => _invocationList;

        public ExecuteData(ExecuteDelegate<T> @delegate, int executeOrder)
        {
            _invocationList = new LinkedList<ExecuteDelegate<T>>();
            ExecuteOrder = executeOrder;

            ExecuteDelegate += @delegate;
            _invocationList.AddLast(@delegate);
        }

        public void AddDelegate(ExecuteDelegate<T> @delegate)
        {
            if (!_invocationList.Contains(@delegate))
            {
                ExecuteDelegate -= @delegate;
                _invocationList.AddLast(@delegate);
            }
        }

        public bool RemoveDelegate(ExecuteDelegate<T> @delegate)
        {
            ExecuteDelegate -= @delegate;
            return _invocationList.Remove(@delegate);
        }

        public void RemoveAllDelegates()
        {
            _invocationList.Clear();
        }

        public bool RemoveTarget(object target)
        {
            LinkedListNode<ExecuteDelegate<T>> node = _invocationList.First;

            do
            {
                if (node.Value.Target.Equals(target))
                {
                    ExecuteDelegate -= node.Value;
                    _invocationList.Remove(node);
                    return true;
                }

                node = node.Next;
            }
            while (node != null);

            return false;
        }
    }
}