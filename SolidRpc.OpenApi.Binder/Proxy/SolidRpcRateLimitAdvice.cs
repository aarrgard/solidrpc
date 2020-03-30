using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.Services.RateLimit;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.Proxy;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcRateLimitAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcRateLimitAdvice(
            ILogger<SolidRpcRateLimitAdvice<TObject, TMethod, TAdvice>> logger,
            ISolidRpcApplication solidApplication,
            ISolidRpcRateLimit rateLimit)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SolidApplication = solidApplication ?? throw new ArgumentNullException(nameof(solidApplication));
            RateLimit = rateLimit ?? throw new ArgumentNullException(nameof(rateLimit));
        }
        private ILogger Logger { get; }
        private ISolidRpcApplication SolidApplication { get; }
        private ISolidRpcRateLimit RateLimit { get; }

        /// <summary>
        /// Configure the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcRateLimitConfig config)
        {
            ResourceName = config.ResourceName;
            Timeout = config.Timeout;
            return true;
        }

        private string ResourceName { get; set; }

        private TimeSpan Timeout { get; set; }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var rateLimitToken = await RateLimit.GetRateLimitTokenAsync(ResourceName, Timeout);
            if(rateLimitToken.Id == Guid.Empty)
            {
                throw new RateLimitExceededException();
            }
            try
            {
                return await next();
            } 
            finally
            {
                await RateLimit.ReturnRateLimitTokenAsync(rateLimitToken);
            }
        }
    }
}
