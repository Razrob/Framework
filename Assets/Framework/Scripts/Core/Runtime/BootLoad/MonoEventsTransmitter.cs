using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class MonoEventsTransmitter : MonoBehaviour, IBootLoadElement, IEventTransmitter<ExecutableMethodID>
    {
        private readonly LinkedList<IEventReceiver<ExecutableMethodID>> _receivers;

        public event ExecuteDelegate OnInitialize;

        public MonoEventsTransmitter()
        {
            _receivers = new LinkedList<IEventReceiver<ExecutableMethodID>>();
        }

        private void Start()
        {
            foreach (IEventReceiver<ExecutableMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableMethodID.OnInitialize);

            foreach (IEventReceiver<ExecutableMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableMethodID.OnBegin);
            
            OnInitialize?.Invoke();
        }

        private void Update()
        {
            foreach (IEventReceiver<ExecutableMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableMethodID.OnUpdate);
        }

        private void FixedUpdate()
        {
            foreach (IEventReceiver<ExecutableMethodID> receiver in _receivers)
                receiver.OnReceive(ExecutableMethodID.OnFixedUpdate);
        }

        public IDisposable Subscribe(IEventReceiver<ExecutableMethodID> eventReceiver)
        {
            _receivers.AddLast(eventReceiver);
            return new UnsubscribeHandler<ExecutableMethodID>(_receivers, eventReceiver);
        }
    }
}