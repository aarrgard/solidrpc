using System;
using Microsoft.Extensions.Logging;

namespace SolidRpc.Test.Petstore.AzFunctions
{
    /// <summary>
    /// Implements logic so that the trace writer output is redirected to the logger.
    /// </summary>
    public class TraceWriterLoggerProvider : ILoggerProvider
    {
        
        private class TraceWriterLogger : ILogger
        {
            public TraceWriterLogger(TraceWriterLoggerProvider traceWriterLoggerProvider)
            {
                TraceWriterLoggerProvider = traceWriterLoggerProvider;
            }

            public TraceWriterLoggerProvider TraceWriterLoggerProvider { get; }

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
                //Console.WriteLine("TraceWriter:"+ formatter(state, exception));
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TraceWriterLogger(this);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}