using System;
using log4net;

namespace CMMAuto.CommonHelp
{
    public class LogHelp
    {
        private static ILog log;

        public LogHelp()
        {
            log = LogManager.GetLogger(typeof(LogHelp));
        }

        public LogHelp(Type clazz)
        {
            log = LogManager.GetLogger(clazz);
        }

        public void Debug(string msg)
        {
            log.Debug(msg);
        }
        public void Info(string msg)
        {
            log.Info(msg);
        }
        public void Warn(string msg)
        {
            log.Warn(msg);
        }
        public void Error(string msg)
        {
            log.Error(msg);
        }
    }
}
