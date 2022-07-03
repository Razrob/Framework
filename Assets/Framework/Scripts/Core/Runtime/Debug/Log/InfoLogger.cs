using UnityEngine;

namespace Framework.Core.Runtime
{
    internal class InfoLogger : LoggerBase
    {
        internal override object Log(string message)
        {
            Debug.Log(message);
            return null;
        }
    }
}