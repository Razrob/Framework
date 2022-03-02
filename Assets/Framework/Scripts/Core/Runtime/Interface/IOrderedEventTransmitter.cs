using System;

namespace Framework.Core.Runtime
{
    public interface IOrderedEventTransmitter
    {
        public IDisposable Subscribe(IEventReceiver eventReceiver, int order = 0);
    }

    public interface IOrderedEventTransmitter<T>
    {
        public IDisposable Subscribe(IEventReceiver<T> eventReceiver, int order = 0);
    }
}