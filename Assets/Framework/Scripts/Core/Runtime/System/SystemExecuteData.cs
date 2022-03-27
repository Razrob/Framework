using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public struct SystemExecuteData
    {
        public IEnumerable<SystemMethodData> MethodsData;
        public FrameworkSystemAttribute FrameworkSystemAttribute;
    }

    public struct SystemMethodData
    {
        public MethodInfo MethodInfo;
        public ExecutableAttribute ExecutableAttribute;

        public SystemMethodData(MethodInfo methodInfo, ExecutableAttribute executableAttribute)
        {
            MethodInfo = methodInfo;
            ExecutableAttribute = executableAttribute;
        }
    }
}