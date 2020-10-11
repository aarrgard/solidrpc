using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using System.Security.Claims;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Invoker;
using System.Linq;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Security.Principal;
using SolidRpc.Abstractions.OpenApi.Proxy;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    public class SecurityPathClaimAdvice
    {
        public static string AllowedPathClaim = "AllowedPath";

        private static IPrincipal _AdminPrincipal;
        private static IEnumerable<Claim> _AdminClaims = new Claim[]
        {
            new Claim(AllowedPathClaim, "/*")
        };
        public static IPrincipal AdminPrincipal
        {
            get
            {
                if (_AdminPrincipal == null)
                {
                    _AdminPrincipal = new ClaimsPrincipal(new ClaimsIdentity(_AdminClaims));
                }
                return _AdminPrincipal;
            }
        }


    }
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityPathClaimAdvice<TObject, TMethod, TAdvice> : SecurityPathClaimAdvice, ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        private static ConcurrentDictionary<string, Regex> PathMatcher = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        /// The security advice should run befor the invocations
        /// </summary>
        public static IEnumerable<Type> BeforeAdvices = new Type[] {
            typeof(SolidRpcOpenApiAdvice<,,>),
        };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SecurityPathClaimAdvice()
        {
        }

        public bool Configure(ISecurityPathClaimConfig config)
        {
            return true;
        }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if (invocation.Caller is ISolidProxy)
            {
                return await next();
            }
            if (invocation.Caller is LocalHandler)
            {
                return await next();
            }
            if (!invocation.SolidProxyInvocationConfiguration.HasImplementation)
            {
                return await next();
            }

            var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
            var prin = auth.CurrentPrincipal as ClaimsPrincipal;
            if(prin == null)
            {
                throw new UnauthorizedException("No principal");
            }
            var methodBinding = invocation.GetValue<IMethodBinding>(typeof(IMethodBinding).FullName);
            if(methodBinding == null)
            {
                throw new UnauthorizedException("No method binding");
            }

            var localPath = methodBinding.LocalPath;
            var match = prin.Claims.Where(o => o.Type == AllowedPathClaim)
                .Select(o => PathMatcher.GetOrAdd(o.Value, CreatePathMatcher))
                .Any(o => o.Match(localPath).Success);

            if(!match)
            {
                throw new UnauthorizedException("No valid path");
            }

            return await next();
        }

        private Regex CreatePathMatcher(string path)
        {
            return new Regex($"^{path.Replace("*", ".*")}$");
        }
    }
}
