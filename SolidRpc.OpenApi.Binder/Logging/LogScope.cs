using SolidRpc.OpenApi.Binder.Logger;
using System;

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
        public LogScope(InvocationLogger memoryLogger, InvocationState invocationState)
        {
            InvocationState = invocationState;
            MemoryLogger = memoryLogger;
            memoryLogger.Push(this);
        }

        internal LogScope Parent { get; set; }

        /// <summary>
        /// The log state
        /// </summary>
        internal InvocationState InvocationState { get; }

        private InvocationLogger MemoryLogger { get; }

        /// <summary>
        /// Disposes of the scope
        /// </summary>
        public void Dispose()
        {
            MemoryLogger.Pop(this);
        }
    }
}
