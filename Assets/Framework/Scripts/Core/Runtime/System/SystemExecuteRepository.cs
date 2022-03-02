using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public class SystemExecuteRepository
    {
        private readonly Dictionary<ExecutableMethodID, Dictionary<int, OrderedExecuteHandler>> _orderedStatedExecuteHandlers;
        private readonly Dictionary<ExecutableMethodID, OrderedExecuteHandler> _orderedNotStatedExecuteHandlers;

        public SystemExecuteRepository(IReadOnlyList<int> statesIDs)
        {
             Array methods = Enum.GetValues(typeof(ExecutableMethodID));
            _orderedStatedExecuteHandlers = new Dictionary<ExecutableMethodID, Dictionary<int, OrderedExecuteHandler>>(methods.Length);
            _orderedNotStatedExecuteHandlers = new Dictionary<ExecutableMethodID, OrderedExecuteHandler>(methods.Length);

            foreach (ExecutableMethodID executableMethod in methods)
            {
                Dictionary<int, OrderedExecuteHandler> states = new Dictionary<int, OrderedExecuteHandler>(statesIDs.Count);

                foreach (int stateID in statesIDs)
                    states.Add(stateID, new OrderedExecuteHandler());

                _orderedStatedExecuteHandlers.Add(executableMethod, states);
                _orderedNotStatedExecuteHandlers.Add(executableMethod, new OrderedExecuteHandler());
            }
        }

        public OrderedExecuteHandler GetStatedExecuteHandler(ExecutableMethodID methodID, int stateID)
        {
            return _orderedStatedExecuteHandlers[methodID][stateID];
        }

        public OrderedExecuteHandler GetNotStatedExecuteHandler(ExecutableMethodID methodID)
        {
            return _orderedNotStatedExecuteHandlers[methodID];
        }
    }
}