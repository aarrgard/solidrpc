using System;
using System.Linq;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// Imements a log scope
    /// </summary>
    public class LogScope : IDisposable
    {
        /// <summary>
        /// Constructs a new log scope
        /// </summary>
        /// <param name="invocationState"></param>
        public LogScope(InvocationLogger invocationLogger, InvocationState invocationState)
        {
            InvocationState = invocationState;
            InvocationLogger = invocationLogger;
            ActivatorPropertyValue = invocationState.Where(o => o.Key == invocationLogger.InvocationLoggerProvider.ActivatorProperty).Select(o => o.Value).FirstOrDefault()?.ToString();
            invocationLogger.InvocationLoggerProvider.Push(this);
        }

        public LogScope ParentScope { get; internal set; }

        /// <summary>
        /// The activator property value
        /// </summary>
        public string ActivatorPropertyValue { get; }

        /// <summary>
        /// The log state
        /// </summary>
        public InvocationState InvocationState { get; }

        public bool IsActivatorScope => ParentScope?.ActivatorPropertyValue != ActivatorPropertyValue;

        private InvocationLogger InvocationLogger { get; }

        /// <summary>
        /// Disposes of the scope
        /// </summary>
        public void Dispose()
        {
            InvocationLogger.InvocationLoggerProvider.Pop(this);
        }
    }
}
