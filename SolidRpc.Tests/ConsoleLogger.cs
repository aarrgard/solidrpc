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
        /// <param name="scope"></param>
        /// <param name="logger"></param>
        public ConsoleLogger(Guid scope, Action<string> logger)
        {
            Logger = logger;
            Scope = scope.ToString().Substring(24);
        }
        private Action<string> Logger { get; }

        /// <summary>
        /// The scope
        /// </summary>
        public string Scope { get; }

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
                Logger($"[{Scope}]{formatter(state, exception)}:{exception.ToString()}");
            }
            else
            {
                Logger($"[{Scope}]{formatter(state, exception)}");
            }
        }
    }
}