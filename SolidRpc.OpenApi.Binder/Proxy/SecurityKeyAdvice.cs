using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;
using SolidRpc.OpenApi.Binder.Invoker;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using System.Security.Principal;
using System.Security.Claims;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityKeyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// The security advice should run befor the invocations
        /// </summary>
        public static IEnumerable<Type> BeforeAdvices = new Type[] {
            typeof(SecurityPathClaimAdvice<,,>)
        };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SecurityKeyAdvice(
            ILogger<SecurityKeyAdvice<TObject, TMethod, TAdvice>> logger)
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
            RemoteCall = !config.InvocationConfiguration.HasImplementation;
            return SecurityKey != null;
        }

        private bool RemoteCall { get; set; }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if(RemoteCall)
            {
                // Add the security key
                if(string.Equals(SecurityKey.Value.Key, "authorization", StringComparison.InvariantCultureIgnoreCase))
                {
                    invocation.SetValue($"http_{SecurityKey.Value.Key}", new StringValues($"Bearer {SecurityKey.Value.Value}"));
                }
                else
                {
                    invocation.SetValue($"http_{SecurityKey.Value.Key}", new StringValues(SecurityKey.Value.Value));
                }
            }
            else
            {
                // Check the security key
                var val = invocation.GetValue<StringValues>($"http_{SecurityKey.Value.Key}".ToLowerInvariant()).ToString();
                if(val != null)
                {
                    if (val.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        val = val.Substring("Bearer ".Length);
                    }
                    if (val.Equals(SecurityKey.Value.Value))
                    {
                        var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
                        auth.CurrentPrincipal = SecurityPathClaimAdvice.AdminPrincipal;
                    }
                }
            }
            return await next();
        }
    }
}
