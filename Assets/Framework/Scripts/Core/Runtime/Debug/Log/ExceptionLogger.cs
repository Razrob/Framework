using System;

namespace Framework.Core.Runtime
{
    internal class ExceptionLogger : LoggerBase
    {
        internal override object Log(string message)
        {
            throw new Exception(message);
            return null;
        }
    }
}