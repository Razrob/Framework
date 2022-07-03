using UnityEngine;

namespace Framework.Core.Runtime
{
    internal class ErrorLogger : LoggerBase
    {
        internal override object Log(string message)
        {
            Debug.LogError(message);
            return null;
        }
    }
}