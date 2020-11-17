using Microsoft.Extensions.Logging;
using SolidRpc.OpenApi.Binder.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Implements the logger that logs to memory storage
    /// </summary>
    public class InvocationLogger : ILogger
    {
        private readonly AsyncLocal<Stack<IEnumerable<KeyValuePair<string, object>>>> _currentScope = new AsyncLocal<Stack<IEnumerable<KeyValuePair<string, object>>>>();

        public InvocationLogger(InvocationLoggingProvider memoryLoggingProvider, string categoryName)
        {
            InvocationLoggerProvider = memoryLoggingProvider;
            CategoryName = categoryName;
        }

        internal void Push(InvocationState invocationState)
        {
            if(_currentScope.Value == null)
            {
                _currentScope.Value = new Stack<IEnumerable<KeyValuePair<string, object>>>();
            }
            _currentScope.Value.Push(invocationState);
            InvocationLoggerProvider.InvocationScopeCreated(invocationState);
        }

        internal void Pop(InvocationState invocationState)
        {
            _currentScope.Value.Pop();
            InvocationLoggerProvider.InvocationScopeDisposed(invocationState);
        }

        private InvocationLoggingProvider InvocationLoggerProvider { get; }
        private string CategoryName { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            var logState = state as IEnumerable<KeyValuePair<string, object>>;
            if (logState == null)
            {
                return null;
            }
            return new LogScope(this, new InvocationState(logState));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logState = _currentScope.Value?.FirstOrDefault();
            InvocationLoggerProvider.Log<TState>(logState, logLevel, eventId, state, exception, formatter);
        }
    }
}