
namespace Framework.Core.Runtime
{
    public interface IEventInvoker
    {
        public void Invoke();
    }

    public interface IEventInvoker<TArg>
    {
        public void Invoke(TArg arg);
    }
    
    public interface IEventInvoker<TArg1, TArg2>
    {
        public void Invoke(TArg1 arg1, TArg2 arg2);
    }

    public interface IEventInvoker<TArg1, TArg2, TArg3>
    {
        public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}