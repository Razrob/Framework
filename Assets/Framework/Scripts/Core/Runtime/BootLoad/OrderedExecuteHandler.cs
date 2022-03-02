using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class OrderedExecuteHandler
    {
        private readonly LinkedList<ExecuteData> _orderedExecuteData = new LinkedList<ExecuteData>();

        public void AddDelegate(ExecuteDelegate @delegate, int order = 0)
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

        public void RemoveDelegate(ExecuteDelegate @delegate)
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                executeData.RemoveDelegate(@delegate);
        }

        public void RemoveAllDelegates()
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                executeData.RemoveAllDelegates();

            _orderedExecuteData.Clear();
        }

        public void RemoveTarget(IExecutable target)
        {
            RemoveTargets(new IExecutable[] { target });
        }

        public void RemoveTargets(IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                foreach (IExecutable target in targets)
                    executeData.RemoveTarget(target);
        }

        public void Execute()
        {
            foreach (ExecuteData executeData in _orderedExecuteData)
                foreach (ExecuteDelegate @delegate in executeData.InvocationList)
                    @delegate.Invoke();
        }

        public void ExecuteSpecific(IExecutable target)
        {
            ExecuteSpecific(new IExecutable[] { target });
        }

        public void ExecuteSpecific(IEnumerable<IExecutable> targets)
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

    public class OrderedExecuteHandler<T>
    {
        private readonly LinkedList<ExecuteData<T>> _orderedExecuteData = new LinkedList<ExecuteData<T>>();

        public void AddDelegate(ExecuteDelegate<T> @delegate, int order = 0)
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

        public void RemoveDelegate(ExecuteDelegate<T> @delegate)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.RemoveDelegate(@delegate);
        }

        public void RemoveAllDelegates()
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.RemoveAllDelegates();

            _orderedExecuteData.Clear();
        }

        public void RemoveTarget(IExecutable target)
        {
            RemoveTargets(new IExecutable[] { target });
        }

        public void RemoveTargets(IEnumerable<IExecutable> targets)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                foreach (IExecutable target in targets)
                    executeData.RemoveTarget(target);
        }

        public void Execute(T arg)
        {
            foreach (ExecuteData<T> executeData in _orderedExecuteData)
                executeData.ExecuteDelegate?.Invoke(arg);
        }

        public void ExecuteSpecific(T arg, IExecutable target)
        {
            ExecuteSpecific(arg, new IExecutable[] { target });
        }

        public void ExecuteSpecific(T arg, IEnumerable<IExecutable> targets)
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