using System;

namespace Framework.Core.Runtime
{
    internal interface IEventTransmitter
    {
        public IDisposable Subscribe(IEventReceiver eventReceiver);
    }

    internal interface IEventTransmitter<T>
    {
        public IDisposable Subscribe(IEventReceiver<T> eventReceiver);
    }
}