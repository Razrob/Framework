using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal struct InjectionData
    {
        internal readonly Type DeclaredType;
        internal readonly List<FieldInfo> InjectionFields;

        internal InjectionData(Type declaredType)
        {
            DeclaredType = declaredType;
            InjectionFields = new List<FieldInfo>();
        }

        internal InjectionData(Type declaredType, IEnumerable<FieldInfo> injectionFields)
        {
            DeclaredType = declaredType;
            InjectionFields = injectionFields.ToList();
        }
    }
}