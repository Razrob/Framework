using System;

namespace Framework.Core.Runtime
{
    public class InternalModelAttribute : Attribute
    {
        internal readonly bool SaveAllow = false;
        internal readonly bool IsSingle = true;

        public InternalModelAttribute() { }
        public InternalModelAttribute(bool saveAllow) => SaveAllow = saveAllow;
        public InternalModelAttribute(bool saveAllow, bool isSingle) : this(saveAllow) => IsSingle = isSingle;
    }
}