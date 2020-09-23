using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    public class SecurityKeyAdvice
    {
        /// <summary>
        /// Checks the security key for the invocation.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="secKey"></param>
        /// <param name="keyFetcher"></param>
        /// <param name="cancellatinToken"></param>
        /// <returns></returns>
        public static Task CheckSecurityKeyAsync(
            object caller,
            KeyValuePair<string, string>? secKey,
            Func<string, string> keyFetcher,
            CancellationToken cancellatinToken = default(CancellationToken))
        {
            if (secKey == null)
            {
                return Task.CompletedTask;
            }

            var key = secKey.Value.Key.ToLower();
            var value = secKey.Value.Value;
            // calls invoked directly from a proxy are allowed
            if (caller is ISolidProxy)
            {
                return Task.CompletedTask;
            }

            //
            // check the security key if specified
            //
            var callKey = keyFetcher(key);
            if (string.IsNullOrEmpty(callKey))
            {
                throw new UnauthorizedException($"No '{key}' specified");
            }
            if (!value.Equals(callKey.ToString()))
            {
                throw new UnauthorizedException("SolidRpcSecurityKey differs");
            }
            return Task.CompletedTask;

        }
    }
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityKeyAdvice<TObject, TMethod, TAdvice> : SecurityKeyAdvice, ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SecurityKeyAdvice(
            ILogger<SolidRpcRateLimitAdvice<TObject, TMethod, TAdvice>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private ILogger Logger { get; }

        private KeyValuePair<string, string>? SecurityKey { get; set; } 

        /// <summary>
        /// Configure the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISecurityKeyConfig config)
        {
            SecurityKey = config.SecurityKey;
            return SecurityKey != null;
        }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            await CheckSecurityKeyAsync(invocation.Caller, SecurityKey, k => invocation.GetValue<StringValues>(k));
            return await next();
        }
    }
}
