using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public struct SystemExecuteData
    {
        public IEnumerable<MethodData> MethodsData;
        public FrameworkSystemAttribute FrameworkSystemAttribute;
    }

    public struct MethodData
    {
        public MethodInfo MethodInfo;
        public ExecutableAttribute ExecutableAttribute;

        public MethodData(MethodInfo methodInfo, ExecutableAttribute executableAttribute)
        {
            MethodInfo = methodInfo;
            ExecutableAttribute = executableAttribute;
        }
    }
}