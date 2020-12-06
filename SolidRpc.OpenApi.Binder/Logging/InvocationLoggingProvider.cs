using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Implements a logging logger provider
    /// </summary>
    public class InvocationLoggingProvider : IDisposable, ILoggerProvider
    {
        private AsyncLocal<LogScope> CurrentLogScope = new AsyncLocal<LogScope>();
        private ConcurrentDictionary<string, ILogger> Loggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="activatorProperty"></param>
        public InvocationLoggingProvider(string activatorProperty = null)
        {
            ActivatorProperty = activatorProperty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ActivatorProperty { get; }
        public InvocationState CurrentScope => CurrentLogScope.Value?.InvocationState ?? InvocationState.EmptyState;

        public event Action<string> InvocationCreatedEvent;
        public event Action<string> InvocationDisposedEvent;
        public event Action<IDictionary<string, string>, LogLevel, EventId, Exception, string> InvocationLogEvent;
        public event Action LoggerDisposeEvent;

        internal void Push(LogScope logScope)
        {
            logScope.ParentScope = CurrentLogScope.Value;
            CurrentLogScope.Value = logScope;
            if (logScope.IsActivatorScope) InvocationScopeCreated(logScope.ActivatorPropertyValue);
        }

        internal void Pop(LogScope logScope)
        {
            if(CurrentLogScope.Value != logScope)
            {
                throw new Exception();
            }
            CurrentLogScope.Value = logScope.ParentScope;
            if (logScope.IsActivatorScope) InvocationScopeDisposed(logScope.ActivatorPropertyValue);
        }

        /// <summary>
        /// Constructs a new logger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return Loggers.GetOrAdd(categoryName, _ => new InvocationLogger(this, categoryName));
        }

        /// <summary>
        /// Disposes of the provider
        /// </summary>
        public void Dispose()
        {
            LoggerDisposeEvent?.Invoke();
        }

        internal void InvocationScopeCreated(string invocationState)
        {
            InvocationCreatedEvent?.Invoke(invocationState);
        }

        internal void InvocationScopeDisposed(string invocationState)
        {
            InvocationDisposedEvent?.Invoke(invocationState);
        }

        internal void Log<TState>(string categoryName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (InvocationLogEvent == null) return;
            var msg = formatter(state, exception);

            var props = new Dictionary<string, string>();
            foreach (var prop in CurrentScope)
            {
                props[prop.Key] = prop.Value?.ToString();
            }
            if(state is IEnumerable<KeyValuePair<string, object>> e)
            {
                foreach (var prop in e)
                {
                    props[prop.Key] = prop.Value?.ToString();
                }
            }
            props["CategoryName"] = categoryName;


            InvocationLogEvent(props, logLevel, eventId, exception, msg);
        }
    }
}
