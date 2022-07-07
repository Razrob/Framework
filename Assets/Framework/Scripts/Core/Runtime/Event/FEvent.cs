using System;

namespace Framework.Core.Runtime
{
    public class FEvent : IEventListener, IEventInvoker
    {
        private event Action _invokationList;

        public IDisposable AddListener(Action action)
        {
            _invokationList += action;
            return new FEventUnsubscribeHanler(ref _invokationList, ref action);
        }

        public void RemoveListener(Action action)
        {
            _invokationList -= action;
        }

        public void Invoke()
        {
            _invokationList?.Invoke();
        }
    }

    public class FEvent<TArg> : IEventListener<TArg>, IEventInvoker<TArg>
    {
        private event Action<TArg> _invokationList;

        public IDisposable AddListener(Action<TArg> action)
        {
            _invokationList += action;
            return new FEventUnsubscribeHanler<TArg>(ref _invokationList, ref action);
        }

        public void RemoveListener(Action<TArg> action)
        {
            _invokationList -= action;
        }

        public void Invoke(TArg arg)
        {
            _invokationList?.Invoke(arg);
        }
    }

    public class FEvent<TArg1, TArg2> : IEventListener<TArg1, TArg2>, IEventInvoker<TArg1, TArg2>
    {
        private event Action<TArg1, TArg2> _invokationList;

        public IDisposable AddListener(Action<TArg1, TArg2> action)
        {
            _invokationList += action;
            return new FEventUnsubscribeHanler<TArg1, TArg2>(ref _invokationList, ref action);
        }

        public void RemoveListener(Action<TArg1, TArg2> action)
        {
            _invokationList -= action;
        }

        public void Invoke(TArg1 arg1, TArg2 arg2)
        {
            _invokationList?.Invoke(arg1, arg2);
        }
    }

    public class FEvent<TArg1, TArg2, TArg3> : IEventListener<TArg1, TArg2, TArg3>, IEventInvoker<TArg1, TArg2, TArg3>
    {
        private event Action<TArg1, TArg2, TArg3> _invokationList;

        public IDisposable AddListener(Action<TArg1, TArg2, TArg3> action)
        {
            _invokationList += action;
            return new FEventUnsubscribeHanler<TArg1, TArg2, TArg3>(ref _invokationList, ref action);
        }

        public void RemoveListener(Action<TArg1, TArg2, TArg3> action)
        {
            _invokationList -= action;
        }

        public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            _invokationList?.Invoke(arg1, arg2, arg3);
        }
    }
}