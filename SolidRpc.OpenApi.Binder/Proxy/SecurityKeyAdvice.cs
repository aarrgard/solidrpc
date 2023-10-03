using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Invoker;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityKeyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static readonly IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SecurityPathClaimAdvice<,,>) };
        public static readonly IEnumerable<Type> AfterAdvices = new Type[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        public SecurityKeyAdvice()
        {
        }
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
            var invocationOptions = InvocationOptions.GetOptions(invocation.SolidProxyInvocationConfiguration.MethodInfo);
            // Check the security key - if supplied

            // add security key(if not set) - this key will be used when invoking remote calls.
            if (invocationOptions.TryGetValue(
                InvocationOptions.RequestHeaderInboundPrefix,
                SecurityKey.Value.Key,
                out string sKkey))
            {
                if (sKkey.Equals(SecurityKey.Value.Value))
                {
                    var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
                    auth.CurrentPrincipal.AddIdentity(SecurityPathClaimAdvice.SecurityKeyIdentity);
                    invocation.ReplaceArgument<IPrincipal>((n, v) => auth.CurrentPrincipal);
                }
            }

            invocationOptions = invocationOptions.SetKeyValue(
                InvocationOptions.RequestHeaderOutboundPrefix,
                SecurityKey.Value.Key,
                SecurityKey.Value.Value);

            using (invocationOptions.Attach())
            {
                return await next();
            }

        }
    }
}
