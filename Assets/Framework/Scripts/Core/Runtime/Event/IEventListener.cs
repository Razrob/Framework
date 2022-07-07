using System;

namespace Framework.Core.Runtime
{
    public interface IEventListener
    {
        public IDisposable AddListener(Action action);
        public void RemoveListener(Action action);
    }

    public interface IEventListener<TArg>
    {
        public IDisposable AddListener(Action<TArg> action);
        public void RemoveListener(Action<TArg> action);
    }

    public interface IEventListener<TArg1, TArg2>
    {
        public IDisposable AddListener(Action<TArg1, TArg2> action);
        public void RemoveListener(Action<TArg1, TArg2> action);
    }

    public interface IEventListener<TArg1, TArg2, TArg3>
    {
        public IDisposable AddListener(Action<TArg1, TArg2, TArg3> action);
        public void RemoveListener(Action<TArg1, TArg2, TArg3> action);
    }
}