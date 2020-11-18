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
            string oldInvocationId = null;
            if(_currentScope.Value.Any())
            {
                var oldScope = _currentScope.Value.Peek() as InvocationState;
                oldInvocationId = oldScope?.InvocationId;
            }
            _currentScope.Value.Push(invocationState);
            if(oldInvocationId != invocationState.InvocationId)
            {
                InvocationLoggerProvider.InvocationScopeCreated(invocationState);
            }
        }

        internal void Pop(InvocationState invocationState)
        {
            var popedScope = _currentScope.Value.Pop() as InvocationState;
            string topInvocationId = null;
            if (_currentScope.Value.Any())
            {
                var topScope = _currentScope.Value.Peek() as InvocationState;
                topInvocationId = topScope?.InvocationId;
            }
            if (popedScope?.InvocationId != topInvocationId)
            {
                InvocationLoggerProvider.InvocationScopeDisposed(invocationState);
            }
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