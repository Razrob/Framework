using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class StatesPreset
    {
        private readonly int[] _statesIndexes;

        internal int StatesCount => _statesIndexes.Length;
        internal IReadOnlyList<int> StatesIndexes => _statesIndexes;

        internal int this[int index] => _statesIndexes[index];

        internal StatesPreset(IEnumerable<int> statesIndexes)
        {
            _statesIndexes = statesIndexes.ToArray();
        }
    }
}