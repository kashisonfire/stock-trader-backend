using System;
using System.Runtime.CompilerServices;

namespace StockTrader.Logger
{
    /// <summary>
    /// Logger for application, based off of log4net
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Configures the logger to use the XML configuration provided by App.config
        /// </summary>
        void Configure();

        /// <summary>
        /// Writes a new error log to database
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Debug(string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

        /// <summary>
        /// Writes a new log to database
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Error(string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

        /// <summary>
        /// Writes a new fatal log to database
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Fatal(string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

        /// <summary>
        /// Writes a new information log to database
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Info(string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

        /// <summary>
        /// Writes a new warn log to database
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Warn(string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

        /// <summary>
        /// Writes a new log to database
        /// </summary>
        /// <param name="level">Level of the log</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        void Write(LogLevel level, string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");
    }
}