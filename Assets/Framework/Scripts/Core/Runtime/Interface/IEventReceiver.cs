
namespace Framework.Core.Runtime
{
    public interface IEventReceiver
    {
        public void OnReceive();
    }

    public interface IEventReceiver<T>
    {
        public void OnReceive(T arg);
    }
}