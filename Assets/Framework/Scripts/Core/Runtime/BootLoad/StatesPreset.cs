using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class StatesPreset
    {
        private readonly int[] _statesIndexes;

        public int StatesCount => _statesIndexes.Length;
        public IReadOnlyList<int> StatesIndexes => _statesIndexes;

        public int this[int index] => _statesIndexes[index];

        public StatesPreset(IEnumerable<int> statesIndexes)
        {
            _statesIndexes = statesIndexes.ToArray();
        }
    }
}