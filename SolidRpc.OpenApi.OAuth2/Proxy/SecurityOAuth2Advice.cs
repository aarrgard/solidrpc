using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Proxy;

namespace SolidRpc.OpenApi.OAuth2.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityOAuth2Advice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        private const string SolidRpcJwtTokenCookieName = "SolidRpcJwtToken";
        /// <summary>
        /// The advices that must run after this advice
        /// </summary>
        public static IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SecurityPathClaimAdvice<,,>), typeof(SecurityPathClaimAdvice<,,>) };

        /// <summary>
        /// The advices that must run before this advice
        /// </summary>
        public static IEnumerable<Type> AfterAdvices = new Type[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public SecurityOAuth2Advice(
            ILogger<SolidRpcRateLimitAdvice<TObject, TMethod, TAdvice>> logger,
            IServiceProvider serviceProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            AuthorityFactory = serviceProvider.GetService<IAuthorityFactory>();
        }
        private ILogger Logger { get; }
        private IAuthorityFactory AuthorityFactory { get; }
        private bool RemoteCall { get; set; }
        private IAuthority Authority { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private IEnumerable<string> Scopes { get; set; }
        private OAuthProxyInvocationPrincipal ProxyInvocationPrincipal { get; set; }
        private bool RedirectUnauthorizedIdentity { get; set; }

        /// <summary>
        /// Configure the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISecurityOAuth2Config config)
        {
            ProxyInvocationPrincipal = config.OAuthProxyInvocationPrincipal;
            Authority = AuthorityFactory?.GetAuthority(config.OAuth2Authority);
            if (Authority == null)
            {
                switch(ProxyInvocationPrincipal)
                {
                    case OAuthProxyInvocationPrincipal.None:
                        // no oaut2 configured
                        return false;
                    case OAuthProxyInvocationPrincipal.Proxy:
                        // we do not need a configured authority to proxy user
                        break;
                    default:
                        if(AuthorityFactory == null)
                        {
                            throw new Exception("No OAuth2Factory found - Is the IoC container configured correctly?");
                        }
                        else
                        {
                            throw new Exception("No OAuth2 Authority configured.");
                        }
                }
            }
            RemoteCall = !config.InvocationConfiguration.HasImplementation;
            ClientId = config.OAuth2ClientId;
            ClientSecret = config.OAuth2ClientSecret;

            if (Scopes == null || !Scopes.Any())
            {
                if(ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Client)
                {
                    Scopes = new[] { "solidrpc-api" };
                }
                else
                {
                    Scopes = new string[0];
                }
            }
            RedirectUnauthorizedIdentity = config.RedirectUnauthorizedIdentity;

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
            var invocationOptions = InvocationOptions.Current;
            if (RemoteCall)
            {
                invocationOptions = await HandleRemoteCall(invocationOptions, invocation);
            }
            else
            {
                invocationOptions = await HandleLocalCall(invocationOptions, invocation);
            }

            using(invocationOptions.Attach())
            {
                return await next();
            }
        }

        private async Task<InvocationOptions> HandleLocalCall(InvocationOptions invocationOptions, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            // 
            // if invocation has been done from a proxy - let the user through
            //
            if (invocation.Caller is ISolidProxy)
            {
                return invocationOptions;
            }

            //
            // check authorization header
            //
            string jwt;
            invocationOptions.TryGetValue($"http_req_authorization", out string authHeader);
            if (string.IsNullOrEmpty(authHeader))
            {
                jwt = await DoRedirectUnauthorizedIdentity(invocationOptions, invocation);
            }
            else if (authHeader.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                jwt = authHeader.Substring("bearer ".Length);
            }
            else if (authHeader.StartsWith("jwt ", StringComparison.InvariantCultureIgnoreCase))
            {
                jwt = authHeader.Substring("jwt ".Length);
            }
            else if (authHeader.StartsWith("basic ", StringComparison.InvariantCultureIgnoreCase))
            {
                jwt = await GetClientJwtAsync(authHeader.Substring("basic ".Length), invocation.CancellationToken);
            }
            else
            {
                jwt = await DoRedirectUnauthorizedIdentity(invocationOptions, invocation);
            }
            if (string.IsNullOrEmpty(jwt))
            {
                return invocationOptions;
            }

            //
            // handle jwt 
            //
            var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
            ClaimsPrincipal jwtPrincipal;
            try
            {
                jwtPrincipal = await Authority.GetPrincipalAsync(jwt, null, invocation.CancellationToken);
            }
            catch (Exception e)
            {
                await DoRedirectUnauthorizedIdentity(invocationOptions, invocation);
                return invocationOptions;
            }

            //
            // assign the principal to the current set of identities
            //
            if (auth.CurrentPrincipal.Claims.Any())
            {
                auth.CurrentPrincipal.AddIdentities(jwtPrincipal.Identities);
            }
            else
            {
                auth.CurrentPrincipal = jwtPrincipal;
            }
            invocation.ReplaceArgument<IPrincipal>((n, v) => auth.CurrentPrincipal);
            
            return invocationOptions;
        }

        private async Task<string> GetClientJwtAsync(string basic, CancellationToken cancellationToken)
        {
            try
            {
                var dec = Encoding.ASCII.GetString(Convert.FromBase64String(basic));
                var parts = dec.Split(':');
                var clientId = parts[0];
                var clientSecret = parts[1];
                var resp = await Authority.GetClientJwtAsync(clientId, clientSecret, Scopes, TimeSpan.FromSeconds(5), cancellationToken);
                return resp.AccessToken;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Invalid basic header:" + basic);
                return null;
            }
        }

        private async Task<string> DoRedirectUnauthorizedIdentity(InvocationOptions invocationOptions, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if(RedirectUnauthorizedIdentity && invocationOptions.TryGetValue<Uri>(MethodInvoker.RequestHeaderMethodUri, out Uri redirectUri))
            {
                //
                // try to fetch token from query
                //
                var accessToken = redirectUri.Query.Split('?').Skip(1).FirstOrDefault()?
                    .Split('&').Select(o => o.Split('='))
                    .Where(o => o.Length > 1)
                    .Where(o => o[0] == "access_token")
                    .Select(o => o[1]).FirstOrDefault();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    return accessToken;
                }
                var authInvoker = invocation.ServiceProvider.GetService<IInvoker<ISolidRpcOAuth2>>();
                var uri = await authInvoker.GetUriAsync(o => o.GetAuthorizationCodeTokenAsync(redirectUri, null, null, invocation.CancellationToken));
                throw new FoundException(uri);
            }
            return null;
        }


        private async Task<InvocationOptions> HandleRemoteCall(InvocationOptions invocationOptions, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.None)
            {
                return invocationOptions;
            }
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Client)
            {
                var jwt = await Authority.GetClientJwtAsync(ClientId, ClientSecret, Scopes);
                if(jwt == null)
                {
                    throw new Exception($"Cannot obtain jwt token for client {ClientId}@{Authority.Authority}.");
                }
                return invocationOptions.SetKeyValue("http_req_authorization", $"bearer {jwt.AccessToken}");
            }
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Proxy)
            {
                var cp = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal;
                var jwt = cp.Claims.Where(o => o.Type == "accesstoken").Select(o => o.Value).FirstOrDefault();
                if(jwt == null)
                {
                    throw new UnauthorizedException("No accesstoken claim exists");
                }
                return invocationOptions.SetKeyValue("http_req_authorization", $"bearer {jwt}");
            }
            throw new NotImplementedException();
        }
    }
}
