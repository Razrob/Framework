using System;

namespace Framework.Core.Runtime
{
    internal interface IOrderedEventTransmitter
    {
        public IDisposable Subscribe(IEventReceiver eventReceiver, int order = 0);
    }

    internal interface IOrderedEventTransmitter<T>
    {
        public IDisposable Subscribe(IEventReceiver<T> eventReceiver, int order = 0);
    }
}