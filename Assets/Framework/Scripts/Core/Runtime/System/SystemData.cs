using System.Collections.Generic;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal struct SystemData
    {
        internal readonly FSystemFoundation System;
        internal readonly Type SystemType;
        internal SystemExecuteData SystemExecuteData;
        internal IEnumerable<FieldInfo> ModelInjectionsFields;
        internal IEnumerable<FieldInfo> InjectionsFields;
        internal IEnumerable<FieldInfo> ComponentSelectorFields;

        internal SystemData(FSystemFoundation systemFoundation, Type type)
        {
            System = systemFoundation;
            SystemType = type;

            SystemExecuteData = new SystemExecuteData();
            ModelInjectionsFields = null;
            InjectionsFields = null; 
            ComponentSelectorFields = null;
        }
    }
}