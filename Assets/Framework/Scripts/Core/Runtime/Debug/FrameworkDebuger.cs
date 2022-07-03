using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal static class FrameworkDebuger
    {
        private static readonly Dictionary<LogType, LoggerBase> _loggers;

        static FrameworkDebuger()
        {
            _loggers = new Dictionary<LogType, LoggerBase>()
            {
                { LogType.Info, new InfoLogger() },
                { LogType.Warning, new WarningLogger() },
                { LogType.Error, new ErrorLogger() },
                { LogType.Exception, new ExceptionLogger() },
            };
        }

        internal static object Log(LogType logType, string message)
        {
            if(_loggers.TryGetValue(logType, out LoggerBase logger))
                return logger.Log($"[Framework] {message}");

            return null;
        }
    }
}