using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using log4net;
using System;
using System.Runtime.CompilerServices;

namespace StockTrader.Logger
{
    public class Logger : ILogger
    {
        private static ILog _logger;
        private readonly IDataService<Log> _logService;
        private bool _isConfigured = false;

        public Logger(IDataService<Log> logService)
        {
            _logService = logService ?? throw new ArgumentException(nameof(logService));
        }

        /// <summary>
        /// Configures the logger to use the XML configuration provided by App.config
        /// </summary>
        public void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
            _isConfigured = true;
        }

        /// <summary>
        /// Writes a new log to database
        /// </summary>
        /// <param name="level">Level of the log</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="caller">Calling method, automatically retreived</param>
        /// <param name="file">File name, automatically retreived</param>
        public void Write(LogLevel level, string message, Exception exception = null, [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            if (!_isConfigured)
            {
                this.Configure();
            }

            _logger = LogManager.GetLogger(System.IO.Path.GetFileNameWithoutExtension(file));
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;

                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;

                case LogLevel.Fatal:
                    _logger.Fatal(message, exception);
                    break;

                case LogLevel.Warn:
                    _logger.Warn(message, exception);
                    break;

                case LogLevel.Info:
                    _logger.Info(message, exception);
                    break;

                default:
                    throw new ArgumentException($"Invalid level type {level}");
            }

            // Log message to database
            _logService.Add(new Log()
            {
                Date = DateTime.Now,
                Thread = caller,
                Level = level.ToString(),
                Logger = _logger.Logger.Name,
                Message = message,
                Exception = exception == null ? "N/A" : exception.ToString()
            });
        }
    }

}
