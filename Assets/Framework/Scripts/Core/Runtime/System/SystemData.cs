using System.Collections.Generic;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public struct SystemData
    {
        public readonly FSystemFoundation System;
        public readonly Type SystemType;
        public SystemExecuteData SystemExecuteData;
        public IEnumerable<FieldInfo> InternalModelFields;
        public IEnumerable<FieldInfo> ComponentSelectorFields;

        public SystemData(FSystemFoundation systemFoundation, Type type)
        {
            System = systemFoundation;
            SystemType = type;

            SystemExecuteData = new SystemExecuteData();
            InternalModelFields = null;
            ComponentSelectorFields = null;
        }
    }
}