using Microsoft.Extensions.Logging;
using System;

namespace SolidRpc.Tests
{
    public class ConsoleProvider : ILoggerProvider
    {
        public ConsoleProvider(Action<string> logger)
        {
            Logger = logger;
        }
        private Action<string> Logger { get; }

        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(Logger);
        }

        public void Dispose()
        {
        }
    }
}