using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Common.Helpers.LogHelper
{
    public static class LogHelper
    {
        public static void WriteLog(LogModel logModel)
        {
            var jsonLog = JsonConvert.SerializeObject(logModel);
            new Logger().Info(jsonLog);
        }
    }
    public class Logger
    {
        private const string FILE_EXT = ".log";
        private readonly string datetimeFormat;
        private readonly string logFilename;

        /// <summary>
        /// Initiate an instance of SimpleLogger class constructor.
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        public Logger()
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            logFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}-{TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")):yyyyMMdd}" + FILE_EXT;

            if (!Directory.Exists($"{dir}\\Log\\"))
            {
                Directory.CreateDirectory($"{dir}\\Log\\");
            }

            // Log file header line
            string logHeader = $"{TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")):yyyyMMdd} daily log is created.";
            if (!System.IO.File.Exists(logFilename))
            {
                WriteLine($"{TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")).ToString(datetimeFormat)} {logHeader}");
            }
        }

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        private void WriteLine(string text, bool append = true)
        {
            try
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter($"{dir}/Log/{logFilename}", append, System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = $"{System.DateTime.Now.ToString(datetimeFormat)} [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }

            WriteLine($"{pretext}{text}");
        }

        [System.Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }
    }

    public static class LogHelperClass
    {
        public static Logger Logger;
        static LogHelperClass()
        {
            if (Logger == null)
            {
                Logger = new Logger();
            }
        }

        public static Logger GetLogger()
        {
            return Logger;
        }

    }
}
