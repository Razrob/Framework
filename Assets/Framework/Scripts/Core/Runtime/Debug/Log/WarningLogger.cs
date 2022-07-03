using UnityEngine;

namespace Framework.Core.Runtime
{
    internal class WarningLogger : LoggerBase
    {
        internal override object Log(string message)
        {
            Debug.LogWarning(message);
            return null;
        }
    }
}