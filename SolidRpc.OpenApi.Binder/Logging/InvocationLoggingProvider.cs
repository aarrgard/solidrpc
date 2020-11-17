using Microsoft.Extensions.Logging;
using SolidRpc.OpenApi.Binder.Logger;
using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Implements a logging logger provider
    /// </summary>
    public class InvocationLoggingProvider : ILoggerProvider
    {
        public event Action<InvocationState> InvocationCreatedEvent;
        public event Action<InvocationState> InvocationDisposedEvent;
        public event Action<IEnumerable<KeyValuePair<string, object>>, LogLevel, EventId, Exception, string> InvocationLogEvent;
        public event Action LoggerDisposeEvent;

        /// <summary>
        /// Constructs a new logger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new InvocationLogger(this, categoryName);
        }

        /// <summary>
        /// Disposes of the provider
        /// </summary>
        public void Dispose()
        {
            LoggerDisposeEvent?.Invoke();
        }

        internal void InvocationScopeCreated(InvocationState invocationState)
        {
            InvocationCreatedEvent?.Invoke(invocationState);
        }

        internal void InvocationScopeDisposed(InvocationState invocationState)
        {
            InvocationDisposedEvent?.Invoke(invocationState);
        }

        internal void Log<TState>(IEnumerable<KeyValuePair<string, object>> invocationState, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (InvocationLogEvent == null) return;
            var msg = formatter(state, exception);
            InvocationLogEvent(invocationState, logLevel, eventId, exception, msg);
        }
    }
}
