using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    internal class MonoEventsTransmitter : MonoBehaviour, IBootLoadElement, IEventTransmitter<ExecutableSystemMethodID>
    {
        private readonly LinkedList<IEventReceiver<ExecutableSystemMethodID>> _receivers;

        internal event ExecuteDelegate OnInitialize;

        internal MonoEventsTransmitter()
        {
            _receivers = new LinkedList<IEventReceiver<ExecutableSystemMethodID>>();
        }

        private void Start()
        {
            foreach (IEventReceiver<ExecutableSystemMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableSystemMethodID.OnInitialize);

            foreach (IEventReceiver<ExecutableSystemMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableSystemMethodID.OnBegin);
            
            OnInitialize?.Invoke();
        }

        private void Update()
        {
            foreach (IEventReceiver<ExecutableSystemMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableSystemMethodID.OnUpdate);
        }

        private void FixedUpdate()
        {
            foreach (IEventReceiver<ExecutableSystemMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableSystemMethodID.OnFixedUpdate);
        }

        public IDisposable Subscribe(IEventReceiver<ExecutableSystemMethodID> eventReceiver)
        {
            _receivers.AddLast(eventReceiver);
            return new UnsubscribeHandler<ExecutableSystemMethodID>(_receivers, eventReceiver);
        }

        public void OnBootLoadComplete() { }
    }
}