using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    public struct InjectionData
    {
        public readonly Type DeclaredType;
        public readonly List<FieldInfo> InjectionFields;

        public InjectionData(Type declaredType)
        {
            DeclaredType = declaredType;
            InjectionFields = new List<FieldInfo>();
        }

        public InjectionData(Type declaredType, IEnumerable<FieldInfo> injectionFields)
        {
            DeclaredType = declaredType;
            InjectionFields = injectionFields.ToList();
        }
    }
}