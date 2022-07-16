using System;

namespace Framework.Core.Runtime
{
    public class FEventUnsubscribeHandler : IDisposable
    {
        private Action _invokationList;
        private readonly Action _action;

        public FEventUnsubscribeHandler(ref Action invokationList, ref Action action)
        {
            _invokationList = invokationList;
            _action = action;
        }

        public void Dispose()
        {
            _invokationList -= _action;
        }
    }

    public class FEventUnsubscribeHandler<TArg> : IDisposable
    {
        private Action<TArg> _invokationList;
        private readonly Action<TArg> _action;

        public FEventUnsubscribeHandler(ref Action<TArg> invokationList, ref Action<TArg> action)
        {
            _invokationList = invokationList;
            _action = action;
        }

        public void Dispose()
        {
            _invokationList -= _action;
        }
    }

    public class FEventUnsubscribeHandler<TArg1, TArg2> : IDisposable
    {
        private Action<TArg1, TArg2> _invokationList;
        private readonly Action<TArg1, TArg2> _action;

        public FEventUnsubscribeHandler(ref Action<TArg1, TArg2> invokationList, ref Action<TArg1, TArg2> action)
        {
            _invokationList = invokationList;
            _action = action;
        }

        public void Dispose()
        {
            _invokationList -= _action;
        }
    }

    public class FEventUnsubscribeHandler<TArg1, TArg2, TArg3> : IDisposable
    {
        private Action<TArg1, TArg2, TArg3> _invokationList;
        private readonly Action<TArg1, TArg2, TArg3> _action;

        public FEventUnsubscribeHandler(ref Action<TArg1, TArg2, TArg3> invokationList, ref Action<TArg1, TArg2, TArg3> action)
        {
            _invokationList = invokationList;
            _action = action;
        }

        public void Dispose()
        {
            _invokationList -= _action;
        }
    }
}
