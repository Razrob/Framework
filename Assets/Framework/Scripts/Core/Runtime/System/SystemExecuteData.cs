using System.Collections.Generic;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal struct SystemExecuteData
    {
        internal IEnumerable<SystemMethodData> MethodsData;
        internal FrameworkSystemAttribute FrameworkSystemAttribute;
    }

    internal struct SystemMethodData
    {
        internal MethodInfo MethodInfo;
        internal ExecutableAttribute ExecutableAttribute;

        internal SystemMethodData(MethodInfo methodInfo, ExecutableAttribute executableAttribute)
        {
            MethodInfo = methodInfo;
            ExecutableAttribute = executableAttribute;
        }
    }
}