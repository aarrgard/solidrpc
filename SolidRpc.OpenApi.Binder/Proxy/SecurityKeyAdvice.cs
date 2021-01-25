using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using System.Security.Principal;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityKeyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SecurityPathClaimAdvice<,,>) };
        public static IEnumerable<Type> AfterAdvices = new Type[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };

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
            // Check the security key - if supplied
            var val = invocation.GetValue<StringValues>($"http_{SecurityKey.Value.Key}").ToString();
            if (val != null)
            {
                if (val.Equals(SecurityKey.Value.Value))
                {
                    var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
                    auth.CurrentPrincipal = SecurityPathClaimAdvice.AdminPrincipal;
                    invocation.ReplaceArgument<IPrincipal>((n, v) => auth.CurrentPrincipal);
                }
            }

            // add security key(if not set)
            invocation.SetValue($"http_{SecurityKey.Value.Key}", new StringValues(SecurityKey.Value.Value));

            return await next();
        }
    }
}
