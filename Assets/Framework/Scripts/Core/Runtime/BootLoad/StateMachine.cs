using System.Collections.Generic;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class StateMachine : IBootLoadElement
    {
        private readonly StatesPreset _statesPreset;

        internal int CurrentState { get; private set; }
        internal IReadOnlyList<int> StatesIndexes => _statesPreset.StatesIndexes;

        internal event StateMachineDelegate<int> OnStateEnter;
        internal event StateMachineDelegate<int> OnStateExit;

        internal StateMachine(StatesPreset statesPreset)
        {
            _statesPreset = statesPreset;
            CurrentState = _statesPreset.StatesIndexes[0];
        }

        internal void SetState(Enum stateEnum)
        {
            try
            {
                int newState = Convert.ToInt32(stateEnum);

                if (!_statesPreset.StatesIndexes.Contains(newState))
                    FrameworkDebuger.Log(LogType.Exception, "[ArgumentException], StateMachine.SetState(), state not found");

                int oldState = CurrentState;
                CurrentState = newState;

                OnStateExit?.Invoke(oldState);
                OnStateEnter?.Invoke(CurrentState);
            }
            catch (InvalidCastException)
            {
                FrameworkDebuger.Log(LogType.Exception, "[InvalidCastException], StateMachine.SetState(), invalid state");
            }
        }

        public void OnBootLoadComplete() { }
    }
}