using System;
using Microsoft.Extensions.Logging;

namespace SolidRpc.Tests
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger(Action<string> logger)
        {
            Logger = logger;
        }
        private Action<string> Logger { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logger(formatter(state, exception));
        }
    }
}