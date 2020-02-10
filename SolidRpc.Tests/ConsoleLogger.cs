using System;
using Microsoft.Extensions.Logging;

namespace SolidRpc.Tests
{
    /// <summary>
    /// A simple console logger
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="logger"></param>
        public ConsoleLogger(Action<string> logger)
        {
            Logger = logger;
        }
        private Action<string> Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(exception != null)
            {
                Logger(formatter(state, exception) + ":" + exception.ToString());
            }
            else
            {
                Logger(formatter(state, exception));
            }
        }
    }
}