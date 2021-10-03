using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KosherUtils.Log
{
    public class Log
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
        public static void Error(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frames = st.GetFrames().Select(f =>
            new
            {
                FileName = f.GetFileName(),
                LineNumber = f.GetFileLineNumber(),
                ColumnNumber = f.GetFileColumnNumber(),
                Method = f.GetMethod(),
                Class = f.GetMethod().DeclaringType,
            });
            foreach (var frame in frames)
            {
                if (string.IsNullOrEmpty(frame.FileName))
                    continue;
                logger.Error($"[ {frame.FileName} : {frame.LineNumber} ] [ {ex.Message} ]");
                logger.Error(frame.FileName, frame.LineNumber);
            }
        }
        public static void Warning(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frames = st.GetFrames().Select(f =>
            new
            {
                FileName = f.GetFileName(),
                LineNumber = f.GetFileLineNumber(),
                ColumnNumber = f.GetFileColumnNumber(),
                Method = f.GetMethod(),
                Class = f.GetMethod().DeclaringType,
            });
            foreach (var frame in frames)
            {
                if (string.IsNullOrEmpty(frame.FileName))
                    continue;

                logger.Warn($"[ {frame.FileName} : {frame.LineNumber} ] [ {ex.Message} ]");
            }
        }
        public static void Debug(string message, [CallerLineNumber] int callerLine = 0, [CallerFilePath] string className = "")
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("message", nameof(className));
            }
            logger.Debug(message, callerLine);
        }
        public static void DebugWriteFile(string message, [CallerLineNumber] int callerLine = 0, [CallerFilePath] string className = "")
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("message", nameof(className));
            }
            logger.Error(message, callerLine);
        }

    }
}
