using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    public class LoadElementAdapter
    {
        protected static readonly List<IBootLoadElement> _bootLoadElements = new List<IBootLoadElement>();
        public static IReadOnlyCollection<IBootLoadElement> BootLoadElements => _bootLoadElements;
    }
}