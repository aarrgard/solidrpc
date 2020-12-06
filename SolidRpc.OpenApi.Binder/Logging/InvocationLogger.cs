using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Implements the logger that logs to memory storage
    /// </summary>
    public class InvocationLogger : ILogger
    {
        public InvocationLogger(InvocationLoggingProvider memoryLoggingProvider, string categoryName)
        {
            InvocationLoggerProvider = memoryLoggingProvider;
            CategoryName = categoryName;
        }

        public InvocationLoggingProvider InvocationLoggerProvider { get; }
        private string CategoryName { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            var ic = new InvocationState(InvocationLoggerProvider.CurrentScope, state as IEnumerable<KeyValuePair<string, object>>);
            return new LogScope(this, ic);
       }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            InvocationLoggerProvider.Log<TState>(CategoryName, logLevel, eventId, state, exception, formatter);
        }
    }
}