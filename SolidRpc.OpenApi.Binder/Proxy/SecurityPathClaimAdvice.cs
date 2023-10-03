using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Invoker;
using System.Linq;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Security.Principal;
using SolidRpc.Abstractions.OpenApi.Proxy;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.InternalServices;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    public class SecurityPathClaimAdvice
    {
        /// <summary>
        /// The name claim
        /// </summary>
        public static readonly Claim NameClaim = new Claim(ClaimsIdentity.DefaultNameClaimType, "SecurityKey");
        
        /// <summary>
        /// The allowed path claim
        /// </summary>
        public static readonly Claim AllowedPathClaim = new Claim("AllowedPath", "/*");

        private static readonly Claim[] SecurityKeyIdentityClaims = new Claim[]
        {
            NameClaim, AllowedPathClaim
        };

        /// <summary>
        /// The identity added to the principal when a security key is supplied.
        /// </summary>
        public static ClaimsIdentity SecurityKeyIdentity
        {
            get
            {
                return new ClaimsIdentity(SecurityKeyIdentityClaims, "SecurityKey");
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
        public SecurityPathClaimAdvice(ILogger<SecurityPathClaimAdvice> logger, IMethodBinderStore methodBinderStore)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private IMethodBinding MethodBinding { get; set; }


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
            var prin = auth.CurrentPrincipal;
            if(!prin.Identities.Any())
            {
                throw new UnauthorizedException("Principal has no identities.");
            }

            var localPath = MethodBinding.LocalPath;
            ClaimsIdentity identityMatch = null;
            foreach (var identity in prin.Identities)
            {
                foreach(var claim in identity.Claims)
                {
                    if (claim.Type != AllowedPathClaim.Type)
                    {
                        continue;
                    }
                    if (!PathMatcher.GetOrAdd(claim.Value, path => CreatePathMatcher(invocation.ServiceProvider, path)).IsMatch(localPath))
                    {
                        continue;
                    }
                    identityMatch = identity;
                    break;
                }
                if(identityMatch != null)
                {
                    break;
                }
            }

            if(identityMatch == null)
            {
                throw new UnauthorizedException($"None of the {AllowedPathClaim.Type} claims matches the path {localPath} for the identities {string.Join(",",prin.Identities.Select(o => o.Name))}");
            }
            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation($"The identity {identityMatch.Name} is allowed to access the path {localPath}.");
            }

            invocation.ReplaceArgument<IPrincipal>((n, v) => prin);

            return await next();
        }

        private Regex CreatePathMatcher(IServiceProvider sp, string path)
        {
            var addressTransformer = sp.GetRequiredService<IMethodAddressTransformer>();
            path = addressTransformer.RewritePath(path);
            return new Regex($"^{path.Replace("*", ".*")}$");
        }
    }
}
