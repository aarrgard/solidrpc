using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
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
                        throw new Exception("No OAuth2 Authority configured.");
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
            if (RemoteCall)
            {
                await HandleRemoteCall(invocation);
            }
            else
            {
                await HandleLocalCall(invocation);
            }

            return await next();
        }

        private async Task HandleLocalCall(ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            // 
            // if invocation has been done from a proxy - let the user through
            //
            if (invocation.Caller is ISolidProxy)
            {
                return;
            }

            //
            // check authorization
            //
            string jwt;
            var authHeader = invocation.GetValue<StringValues>($"http_req_authorization").ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                jwt = await DoRedirectUnauthorizedIdentity(invocation, true);
            }
            else if (authHeader.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                jwt = authHeader.Substring("bearer ".Length);
            }
            else if (authHeader.StartsWith("jwt ", StringComparison.InvariantCultureIgnoreCase))
            {
                jwt = authHeader.Substring("jwt ".Length);
            }
            else
            {
                jwt = await DoRedirectUnauthorizedIdentity(invocation, true);
            }
            if(string.IsNullOrEmpty(jwt))
            {
                return;
            }
            var auth = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
            ClaimsPrincipal jwtPrincipal;
            try
            {
                jwtPrincipal = await Authority.GetPrincipalAsync(jwt, null, invocation.CancellationToken);
            }
            catch (Exception e)
            {
                await DoRedirectUnauthorizedIdentity(invocation, false);
                return;
            }
            if (auth.CurrentPrincipal.Claims.Any())
            {
                auth.CurrentPrincipal.AddIdentities(jwtPrincipal.Identities);
            }
            else
            {
                auth.CurrentPrincipal = jwtPrincipal;
            }
            invocation.ReplaceArgument<IPrincipal>((n, v) => auth.CurrentPrincipal);
        }

        private async Task<string> DoRedirectUnauthorizedIdentity(ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation, bool useCookie)
        {
            if(RedirectUnauthorizedIdentity)
            {
                var redirectUri = new Uri(invocation.GetValue<StringValues>($"{MethodInvoker.RequestHeaderPrefixInInvocation}x-orig-uri").ToString());
                //
                // try to fetch token from query
                //
                var idToken = redirectUri.Query.Split('?').Skip(1).FirstOrDefault()?
                    .Split('&').Select(o => o.Split('=')).Where(o => o.Length > 1).Where(o => o[0] == "id_token").Select(o => o[1]).FirstOrDefault();
                if (!string.IsNullOrEmpty(idToken))
                {
                    var secure = string.Equals(redirectUri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) ? "; Secure" : "";
                    invocation.SetValue($"{MethodInvoker.ResponseHeaderPrefixInInvocation}Set-Cookie", $"{SolidRpcJwtTokenCookieName}={idToken}{secure}; SameSite=Strict; path=/");
                    var ub1 = new UriBuilder(redirectUri);
                    ub1.Query = null;
                    throw new FoundException(ub1.Uri);
                    //return idToken;
                }

                idToken = GetTokenFromCookie(invocation, useCookie);
                if (!string.IsNullOrEmpty(idToken))
                {
                    return idToken;
                }

                //
                // Redirect to auth server
                //
                var doc = await Authority.GetDiscoveryDocumentAsync(invocation.CancellationToken);
                var ub = new UriBuilder(doc.AuthorizationEndpoint);
                ub.Query = $"response_type=id_token&client_id={HttpUtility.UrlEncode(ClientId)}&client_secret={HttpUtility.UrlEncode(ClientSecret)}&scopes={string.Join(" ", Scopes.Select(o => HttpUtility.UrlEncode(o)))}&redirect_uri={HttpUtility.UrlEncode(redirectUri.ToString())}";
                throw new FoundException(ub.Uri);
            }
            return GetTokenFromCookie(invocation, useCookie);
        }

        private string GetTokenFromCookie(ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation, bool useCookie)
        {
            if(!useCookie)
            {
                return null;
            }
            var cookieValues = invocation.GetValue<StringValues>($"{MethodInvoker.RequestHeaderPrefixInInvocation}Cookie").ToString();
            foreach (var val in cookieValues.Split(';'))
            {
                var values = val.Split('=').Select(o => o.Trim());
                if (values.First() == SolidRpcJwtTokenCookieName)
                {
                    return values.Skip(1).FirstOrDefault();
                }
            }
            return null;
        }

        private async Task HandleRemoteCall(ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.None)
            {
                return;
            }
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Client)
            {
                var jwt = await Authority.GetClientJwtAsync(ClientId, ClientSecret, Scopes);
                if(jwt == null)
                {
                    throw new Exception($"Cannot obtain jwt token for client {ClientId}@{Authority.Authority}.");
                }
                invocation.SetValue<StringValues>("http_req_authorization", $"bearer {jwt}");
                return;
            }
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Proxy)
            {
                var cp = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal;
                var jwt = cp.Claims.Where(o => o.Type == "accesstoken").Select(o => o.Value).FirstOrDefault();
                if(jwt == null)
                {
                    throw new UnauthorizedException("No accesstoken claim exists");
                }
                invocation.SetValue<StringValues>("http_req_authorization", $"bearer {jwt}");
                return;
            }
            throw new NotImplementedException();
        }
    }
}
