using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class StateMachine : IBootLoadElement
    {
        private readonly StatesPreset _statesPreset;

        public int CurrentState { get; private set; }
        public IReadOnlyList<int> StatesIndexes => _statesPreset.StatesIndexes;

        public event StateMachineDelegate<int> OnStateEnter;
        public event StateMachineDelegate<int> OnStateExit;

        public StateMachine(StatesPreset statesPreset)
        {
            _statesPreset = statesPreset;
            CurrentState = _statesPreset.StatesIndexes[0];
        }

        public void SetState(Enum stateEnum)
        {
            try
            {
                int newState = Convert.ToInt32(stateEnum);

                if (!_statesPreset.StatesIndexes.Contains(newState))
                    throw new ArgumentException("State not found");

                int oldState = CurrentState;
                CurrentState = newState;

                OnStateExit?.Invoke(oldState);
                OnStateEnter?.Invoke(CurrentState);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException("Invalid state");
            }
        }
    }
}