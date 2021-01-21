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
using SolidRpc.Abstractions.OpenApi.Invoker;

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
        public static IEnumerable<Type> AfterAdvices = new[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };
        public static IEnumerable<Type> BeforeAdvices = new[] { typeof(SolidRpcOpenApiInvocAdvice<,,>) };

        private static ConcurrentDictionary<string, Regex> PathMatcher = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SecurityPathClaimAdvice(IMethodBinderStore methodBinderStore)
        {
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
        }

        private IMethodBinderStore MethodBinderStore { get; }
        public IMethodBinding MethodBinding { get; private set; }


        public bool Configure(ISecurityPathClaimConfig config)
        {
            //
            // get binding
            //
            var apiConfig = config.GetAdviceConfig<ISolidRpcOpenApiConfig>();
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                apiConfig.OpenApiSpec,
                apiConfig.InvocationConfiguration.MethodInfo,
                apiConfig.GetTransports()
            ).First();

            return true;
        }

        /// <summary>
        /// Handles the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var handler = invocation.GetValue<IHandler>(typeof(IHandler).FullName) ?? throw new Exception("No handler assigned to invocation");
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

            var localPath = MethodBinding.LocalPath;
            var match = prin.Claims.Where(o => o.Type == AllowedPathClaim)
                .Select(o => PathMatcher.GetOrAdd(o.Value, CreatePathMatcher))
                .Any(o => o.Match(localPath).Success);

            if(!match)
            {
                throw new UnauthorizedException("No valid path");
            }

            invocation.ReplaceArgument<IPrincipal>((n, v) => prin);

            return await next();
        }

        private Regex CreatePathMatcher(string path)
        {
            return new Regex($"^{path.Replace("*", ".*")}$");
        }
    }
}
