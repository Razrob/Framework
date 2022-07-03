using System.Collections.Generic;
using System;

namespace Framework.Core.Runtime
{
    internal class SystemExecuteRepository
    {
        private readonly Dictionary<ExecutableSystemMethodID, Dictionary<int, OrderedExecuteHandler>> _orderedStatedExecuteHandlers;
        private readonly Dictionary<ExecutableSystemMethodID, OrderedExecuteHandler> _orderedNotStatedExecuteHandlers;

        internal SystemExecuteRepository(IReadOnlyList<int> statesIDs)
        {
             Array methods = Enum.GetValues(typeof(ExecutableSystemMethodID));
            _orderedStatedExecuteHandlers = new Dictionary<ExecutableSystemMethodID, Dictionary<int, OrderedExecuteHandler>>(methods.Length);
            _orderedNotStatedExecuteHandlers = new Dictionary<ExecutableSystemMethodID, OrderedExecuteHandler>(methods.Length);

            foreach (ExecutableSystemMethodID executableMethod in methods)
            {
                Dictionary<int, OrderedExecuteHandler> states = new Dictionary<int, OrderedExecuteHandler>(statesIDs.Count);

                foreach (int stateID in statesIDs)
                    states.Add(stateID, new OrderedExecuteHandler());

                _orderedStatedExecuteHandlers.Add(executableMethod, states);
                _orderedNotStatedExecuteHandlers.Add(executableMethod, new OrderedExecuteHandler());
            }
        }

        internal OrderedExecuteHandler GetStatedExecuteHandler(ExecutableSystemMethodID methodID, int stateID)
        {
            if (!_orderedStatedExecuteHandlers.ContainsKey(methodID))
                return null;

            return _orderedStatedExecuteHandlers[methodID][stateID];
        }

        internal OrderedExecuteHandler GetNotStatedExecuteHandler(ExecutableSystemMethodID methodID)
        {
            if (!_orderedNotStatedExecuteHandlers.ContainsKey(methodID))
                return null;

            return _orderedNotStatedExecuteHandlers[methodID];
        }
    }
}