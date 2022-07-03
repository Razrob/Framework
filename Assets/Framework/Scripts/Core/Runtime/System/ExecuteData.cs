using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal class ExecuteData
    {
        private readonly LinkedList<ExecuteDelegate> _invocationList;

        internal readonly int ExecuteOrder;
        internal ExecuteDelegate ExecuteDelegate;
        internal IEnumerable<ExecuteDelegate> InvocationList => _invocationList;

        internal ExecuteData(ExecuteDelegate @delegate, int executeOrder)
        {
            _invocationList = new LinkedList<ExecuteDelegate>();
            ExecuteOrder = executeOrder;

            ExecuteDelegate += @delegate;
            _invocationList.AddLast(@delegate);
        }

        internal void AddDelegate(ExecuteDelegate @delegate)
        {
            if (!_invocationList.Contains(@delegate))
            {
                ExecuteDelegate += @delegate;
                _invocationList.AddLast(@delegate);
            }
        }

        internal bool RemoveDelegate(ExecuteDelegate @delegate)
        {
            ExecuteDelegate -= @delegate;
            return _invocationList.Remove(@delegate);
        }

        internal void RemoveAllDelegates()
        {
            ExecuteDelegate = null;
            _invocationList.Clear();
        }

        internal bool RemoveTarget(object target)
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

    internal class ExecuteData<T>
    {
        private readonly LinkedList<ExecuteDelegate<T>> _invocationList;

        internal readonly int ExecuteOrder;
        internal ExecuteDelegate<T> ExecuteDelegate;
        internal IEnumerable<ExecuteDelegate<T>> InvocationList => _invocationList;

        internal ExecuteData(ExecuteDelegate<T> @delegate, int executeOrder)
        {
            _invocationList = new LinkedList<ExecuteDelegate<T>>();
            ExecuteOrder = executeOrder;

            ExecuteDelegate += @delegate;
            _invocationList.AddLast(@delegate);
        }

        internal void AddDelegate(ExecuteDelegate<T> @delegate)
        {
            if (!_invocationList.Contains(@delegate))
            {
                ExecuteDelegate -= @delegate;
                _invocationList.AddLast(@delegate);
            }
        }

        internal bool RemoveDelegate(ExecuteDelegate<T> @delegate)
        {
            ExecuteDelegate -= @delegate;
            return _invocationList.Remove(@delegate);
        }

        internal void RemoveAllDelegates()
        {
            _invocationList.Clear();
        }

        internal bool RemoveTarget(object target)
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