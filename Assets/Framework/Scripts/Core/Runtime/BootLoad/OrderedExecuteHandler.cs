using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class OrderedExecuteHandler
    {
        private readonly LinkedList<ExecuteData> _orderedExecuteData = new LinkedList<ExecuteData>();

        internal void AddDelegate(ExecuteDelegate @delegate, int order = 0)
        {
            if (_orderedExecuteData.Count == 0)
            {
                _orderedExecuteData.AddFirst(new ExecuteData(@delegate, order));
                return;
            }

            LinkedListNode<ExecuteData> node = _orderedExecuteData.First;

            do
            {
                if (node.Value.ExecuteOrder == order)
                {
                    node.Value.AddDelegate(@delegate);
                    return;
                }

                if (node.Value.ExecuteOrder < order && (node.Next == null || node.Next.Value.ExecuteOrder > order))
                {
                    _orderedExecuteData.AddAfter(node, new ExecuteData(@delegate, order));
                    return;
                }

                if(node.Value.ExecuteOrder > order)
                {
                    _orderedExecuteData.AddBefore(node, new ExecuteData(@delegate, order));
                    return;
                }

                node = node.Next;
            }
            while (node != null);
        }

        internal void RemoveDelegate(ExecuteDelegate @delegate)
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                executeData.RemoveDelegate(@delegate);
        }

        internal void RemoveAllDelegates()
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                executeData.RemoveAllDelegates();

            _orderedExecuteData.Clear();
        }

        internal void RemoveTarget(IExecutable target)
        {
            RemoveTargets(new IExecutable[] { target });
        }

        internal void RemoveTargets(IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                foreach (IExecutable target in targets)
                    executeData.RemoveTarget(target);
        }

        internal void Execute()
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                foreach (ExecuteDelegate @delegate in executeData.InvocationList)
                    @delegate.Invoke();
        }

        internal void ExecuteSpecific(IExecutable target)
        {
            ExecuteSpecific(new IExecutable[] { target });
        }

        internal void ExecuteSpecific(IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
            {
                foreach (ExecuteDelegate @delegate in executeData.InvocationList)
                {
                    if (!targets.Contains(@delegate.Target))
                        continue;

                    @delegate.Invoke();
                }
            }
        }
    }

    internal class OrderedExecuteHandler<T>
    {
        private readonly LinkedList<ExecuteData<T>> _orderedExecuteData = new LinkedList<ExecuteData<T>>();

        internal void AddDelegate(ExecuteDelegate<T> @delegate, int order = 0)
        {
            if (_orderedExecuteData.Count == 0)
            {
                _orderedExecuteData.AddFirst(new ExecuteData<T>(@delegate, order));
                return;
            }

            LinkedListNode<ExecuteData<T>> node = _orderedExecuteData.First;

            do
            {
                if (node.Value.ExecuteOrder == order)
                {
                    node.Value.AddDelegate(@delegate);
                    return;
                }

                if (node.Value.ExecuteOrder < order && (node.Next == null || node.Next.Value.ExecuteOrder > order))
                {
                    _orderedExecuteData.AddAfter(node, new ExecuteData<T>(@delegate, order));
                    return;
                }

                if (node.Value.ExecuteOrder > order)
                {
                    _orderedExecuteData.AddBefore(node, new ExecuteData<T>(@delegate, order));
                    return;
                }

                node = node.Next;
            }
            while (node != null);
        }

        internal void RemoveDelegate(ExecuteDelegate<T> @delegate)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.RemoveDelegate(@delegate);
        }

        internal void RemoveAllDelegates()
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.RemoveAllDelegates();

            _orderedExecuteData.Clear();
        }

        internal void RemoveTarget(IExecutable target)
        {
            RemoveTargets(new IExecutable[] { target });
        }

        internal void RemoveTargets(IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                foreach (IExecutable target in targets)
                    executeData.RemoveTarget(target);
        }

        internal void Execute(T arg)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.ExecuteDelegate?.Invoke(arg);
        }

        internal void ExecuteSpecific(T arg, IExecutable target)
        {
            ExecuteSpecific(arg, new IExecutable[] { target });
        }

        internal void ExecuteSpecific(T arg, IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
            {
                foreach (ExecuteDelegate<T> @delegate in executeData.InvocationList)
                {
                    if (!targets.Contains(@delegate.Target))
                        continue;

                    @delegate?.Invoke(arg);
                }
            }
        }
    }
}