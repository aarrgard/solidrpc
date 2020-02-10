using Microsoft.Extensions.Logging;
using System;

namespace SolidRpc.Tests
{
    /// <summary>
    /// A simple console provider
    /// </summary>
    public class ConsoleProvider : ILoggerProvider
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="logger"></param>
        public ConsoleProvider(Action<string> logger)
        {
            Logger = logger;
        }
        private Action<string> Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(Logger);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}