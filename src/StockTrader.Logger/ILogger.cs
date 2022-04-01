using System;
using System.Runtime.CompilerServices;

namespace StockTrader.Logger
{
    /// <summary>
    /// TODO: 
    /// - Possibly make this async
    /// - Add PLC logging support
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Configures the logger to use the XML configuration provided by App.config
        /// </summary>
        void Configure();

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