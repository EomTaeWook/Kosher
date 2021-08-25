namespace KosherUtils.Log
{
    class Log
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Debug(string logMessage)
        {
            logger.Debug(logMessage);
        }
        public static void Info(string logMessage)
        {
            logger.Info(logMessage);
        }
        public static void Error(string logMessage)
        {
            logger.Error(logMessage);
        }
    }
}
