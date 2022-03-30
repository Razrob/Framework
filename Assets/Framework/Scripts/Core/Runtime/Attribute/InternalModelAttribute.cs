﻿using System;

namespace Framework.Core.Runtime
{
    public class InternalModelAttribute : Attribute
    {
        public readonly bool SaveAllow = false;

        public InternalModelAttribute() { }
        public InternalModelAttribute(bool saveAllow) => SaveAllow = saveAllow;
    }
}