using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    public class LoggingAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public LoggingAdvice(ILogger<LoggingAdvice<TObject, TMethod, TAdvice>> logger)
        {
            Logger = logger;
        }
        private ILogger Logger { get; }

        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var methodInfo = invocation.SolidProxyInvocationConfiguration.MethodInfo;
            try
            {
                Logger.LogTrace("Entering " + methodInfo.Name);
                return await next();
            }
            finally
            {
                Logger.LogTrace("Exiting " + methodInfo.Name);
            }
        }
    }
}
