using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Runtime
{
    public static class SystemExecuteDataExtractor
    {
        private static readonly BindingFlags ExecutableMethodsFlag = BindingFlags.Instance | BindingFlags.NonPublic;

        public static SystemExecuteData GetExecuteData(Type systemType)
        {
            if (!systemType.IsSubclassOf(typeof(FSystemFoundation)))
                throw new ArgumentException("Incorrect system type");

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

        public static FrameworkSystemAttribute GetSystemAttribute(Type systemType)
        {
            if (!systemType.IsSubclassOf(typeof(FSystemFoundation)))
                throw new ArgumentException("Incorrect system type");

            return systemType.GetCustomAttribute<FrameworkSystemAttribute>();
        }
    }
}