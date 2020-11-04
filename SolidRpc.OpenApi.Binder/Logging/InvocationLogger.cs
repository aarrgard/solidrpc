using Microsoft.Extensions.Logging;
using SolidRpc.OpenApi.Binder.Logger;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Implements the logger that logs to memory storage
    /// </summary>
    public class InvocationLogger : ILogger
    {
        private readonly AsyncLocal<LogScope> _currentScope = new AsyncLocal<LogScope>();

        public InvocationLogger(InvocationLoggingProvider memoryLoggingProvider, string categoryName)
        {
            MemoryLoggingProvider = memoryLoggingProvider;
            CategoryName = categoryName;
        }

        internal void Push(LogScope logScope)
        {
            logScope.Parent = _currentScope.Value;
            _currentScope.Value = logScope;
            MemoryLoggingProvider.InvocationScopeCreated(logScope.InvocationState);
        }

        internal void Pop(LogScope logScope)
        {
            _currentScope.Value = logScope.Parent;
            MemoryLoggingProvider.InvocationScopeDisposed(logScope.InvocationState);
        }

        private InvocationLoggingProvider MemoryLoggingProvider { get; }
        private string CategoryName { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            var logState = state as InvocationState;
            if(logState == null)
            {
                return null;
            }
            _currentScope.Value = new LogScope(this, logState);
            return _currentScope.Value;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logState = _currentScope.Value?.InvocationState;
            MemoryLoggingProvider.Log<TState>(logState, logLevel, eventId, state, exception, formatter);
        }
    }
}