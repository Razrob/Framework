using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    internal class UnsubscribeHandler : IDisposable
    {
        private readonly ICollection<IEventReceiver> _receivers;
        private readonly IEventReceiver _unsubscribedReceiver;

        internal UnsubscribeHandler(ICollection<IEventReceiver> receivers, IEventReceiver unsubscribedReceiver)
        {
            _receivers = receivers;
            _unsubscribedReceiver = unsubscribedReceiver;
        }

        public void Dispose()
        {
            _receivers.Remove(_unsubscribedReceiver);
        }
    }

    internal class UnsubscribeHandler<T> : IDisposable
    {
        private readonly ICollection<IEventReceiver<T>> _receivers;
        private readonly IEventReceiver<T> _unsubscribedReceiver;

        internal UnsubscribeHandler(ICollection<IEventReceiver<T>> receivers, IEventReceiver<T> unsubscribedReceiver)
        {
            _receivers = receivers;
            _unsubscribedReceiver = unsubscribedReceiver;
        }

        public void Dispose()
        {
            _receivers.Remove(_unsubscribedReceiver);
        }
    }
}