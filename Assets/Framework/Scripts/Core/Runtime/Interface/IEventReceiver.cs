
namespace Framework.Core.Runtime
{
    internal interface IEventReceiver
    {
        public void OnReceive();
    }

    internal interface IEventReceiver<T>
    {
        public void OnReceive(T arg);
    }
}