using System;

namespace Framework.Core.Runtime
{
    public interface IEventTransmitter
    {
        public IDisposable Subscribe(IEventReceiver eventReceiver);
    }

    public interface IEventTransmitter<T>
    {
        public IDisposable Subscribe(IEventReceiver<T> eventReceiver);
    }
}