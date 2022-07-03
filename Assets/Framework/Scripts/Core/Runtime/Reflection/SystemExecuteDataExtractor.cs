using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal static class SystemExecuteDataExtractor
    {
        private static readonly BindingFlags ExecutableMethodsFlag = BindingFlags.Instance | BindingFlags.NonPublic;

        internal static SystemExecuteData GetExecuteData(Type systemType)
        {
            if (!systemType.IsSubclassOf(typeof(FSystemFoundation)))
                return (SystemExecuteData)FrameworkDebuger.Log(LogType.Exception, "[ArgumentException], incorrect system type");

            string[] executableMethodsNames = Enum.GetNames(typeof(ExecutableSystemMethodID));

            SystemExecuteData systemExecuteData = new SystemExecuteData();
            systemExecuteData.FrameworkSystemAttribute = GetSystemAttribute(systemType);

            List<SystemMethodData> methodsData = new List<SystemMethodData>();

            foreach (MethodInfo methodInfo in systemType.GetMethods(ExecutableMethodsFlag))
            {
                if (!executableMethodsNames.Contains(methodInfo.Name))
                    continue;

                ExecutableAttribute executableAttribute = methodInfo.GetCustomAttribute<ExecutableAttribute>();

                if (executableAttribute != null && executableAttribute.Execute)
                    methodsData.Add(new SystemMethodData(methodInfo, executableAttribute));
            }

            systemExecuteData.MethodsData = methodsData;

            return systemExecuteData;
        }

        internal static FrameworkSystemAttribute GetSystemAttribute(Type systemType)
        {
            if (!systemType.IsSubclassOf(typeof(FSystemFoundation)))
                return (FrameworkSystemAttribute)FrameworkDebuger.Log(LogType.Exception, "[ArgumentException], incorrect system type");

            return systemType.GetCustomAttribute<FrameworkSystemAttribute>();
        }
    }
}