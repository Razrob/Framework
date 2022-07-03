using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal class LoadElementAdapter
    {
        protected static readonly List<IBootLoadElement> _bootLoadElements = new List<IBootLoadElement>();
        internal static IReadOnlyCollection<IBootLoadElement> BootLoadElements => _bootLoadElements;
    }
}