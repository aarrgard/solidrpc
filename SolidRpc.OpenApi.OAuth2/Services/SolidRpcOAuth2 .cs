using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using SolidRpc.OpenApi.Binder.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

[assembly: SolidRpcService(typeof(ISolidRpcOAuth2), typeof(SolidRpcOAuth2), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Services
{
    /// <summary>
    /// Implements the solid authorization logic
    /// </summary>
    public class SolidRpcOAuth2 : ISolidRpcOAuth2
    {
        private const string RefreshTokenCookieName = "RefreshToken";

        private class TokenData
        {
            [DataMember(Name = "iss")]
            public string Iss { get; set; }
            [DataMember(Name = "sub")]
            public string Sub { get; set; }
            [DataMember(Name = "client_id")]
            public string ClientId { get; set; }
        }

        private class State
        {
            public string ClientState { get; set; }
            public Uri Callback { get; set; }
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcOAuth2(
            IServiceProvider serviceProvider,
            IInvoker<ISolidRpcOAuth2> invoker,
            ISerializerFactory serializationFactory)
        {
            ServiceProvider = serviceProvider;
            Invoker = invoker;
            SerializationFactory = serializationFactory;
        }

        private IServiceProvider ServiceProvider { get; }
        private IInvoker<ISolidRpcOAuth2> Invoker { get; }
        private ISerializerFactory SerializationFactory { get; }
        private IMethodAddressTransformer MethodAddressTransformer => ServiceProvider.GetRequiredService<IMethodAddressTransformer>();
        private IEnumerable<string> AllowedHosts {
            get
            {
                return MethodAddressTransformer.AllowedCorsOrigins
                    .Select(o =>
                    {
                        if (Uri.TryCreate(o, UriKind.Absolute, out Uri res))
                        {
                            return res.Host;
                        }
                        else
                        {
                            return null;
                        }
                    }).Where(o => o != null)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns the authorization code token
        /// </summary>
        /// <param name="callbackUri"></param>
        /// <param name="state"></param>
        /// <param name="scopes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> GetAuthorizationCodeTokenAsync(
            Uri callbackUri = null, 
            string state = null,
            IEnumerable<string> scopes = null,
            CancellationToken cancellationToken = default)
        {
            CheckAllowedCallback(callbackUri);

            var conf = GetOAuth2Conf();
            var auth = GetAuthority(conf);
            var doc = await GetDiscoveryDocumentAsync(conf, cancellationToken);

            scopes = auth.GetScopes("authorization_code", scopes);

            var statems = new MemoryStream();
            SerializationFactory.SerializeToStream(statems, new State()
            {
                ClientState = state,
                Callback = callbackUri
            });

            var redirectUri = await Invoker.GetUriAsync(o => o.TokenCallbackAsync(null, null, cancellationToken));
            var authorizationEndpoint = doc.AuthorizationEndpoint;
            var client_id = HttpUtility.UrlEncode(conf.OAuth2ClientId);
            var redirect_uri = HttpUtility.UrlEncode(redirectUri.ToString());
            var serverState = HttpUtility.UrlEncode(Convert.ToBase64String(statems.ToArray()));
            var scope = HttpUtility.UrlEncode(string.Join(" ", scopes));

            if(doc.AuthorizationEndpoint == null)
            {
                throw new Exception($"Authority {auth.Authority} does not have an authorization endpoint configured.");
            }

            if (scopes.Any(o => o.Equals("offline_access", StringComparison.InvariantCultureIgnoreCase)))
            {
                var refreshUri = await Invoker.GetUriAsync(o => o.RefreshTokenAsync("current", cancellationToken));
                return await CreateContent(nameof(GetAuthorizationCodeTokenAsync), new Dictionary<string, string>()
                {
                    { "refreshTokenEndpoint", $"{refreshUri}"},
                    { "callback_uri", callbackUri.ToString() },
                    { "clientState", state},
                    { "authorizationEndpoint", $"{doc.AuthorizationEndpoint}"},
                    { "client_id", client_id},
                    { "serverState", serverState},
                    { "scope", scope },
                    { "redirect_uri", redirect_uri}
                });
            }
            else
            {
                var uri = new Uri($"{authorizationEndpoint}?response_type=code&client_id={client_id}&redirect_uri={redirect_uri}&state={serverState}&scope={scope}");
                throw new FoundException(uri);
            }

        }

        private void CheckAllowedCallback(Uri callbackUri)
        {
            callbackUri = callbackUri ?? new Uri("https://not.supplied/");
            var host = callbackUri.Host.Split(':').First();
            if (!AllowedHosts.Contains(host))
            {
                throw new ArgumentException($"Host not allowed({host}). Allowed hosts are {string.Join(",", AllowedHosts)}");
            }
        }

        private ISecurityOAuth2Config GetOAuth2Conf()
        {
            var pc = SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice.CurrentInvocation.SolidProxyInvocationConfiguration;
            if (!pc.IsAdviceConfigured<ISecurityOAuth2Config>())
            {
                throw new Exception("No authority configured");
            }
            return pc.ConfigureAdvice<ISecurityOAuth2Config>();
        }

        private IAuthority GetAuthority(ISecurityOAuth2Config conf)
        {
            var issuer = conf.OAuth2Authority;
            var authFact = ServiceProvider.GetService<IAuthorityFactory>();
            if (authFact == null)
            {
                throw new Exception("No authority factory configured in IoC.");
            }
            return authFact.GetAuthority(issuer);
        }

        private async Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(ISecurityOAuth2Config conf, CancellationToken cancellationToken)
        {
            var dd = await GetAuthority(conf).GetDiscoveryDocumentAsync(cancellationToken);
            if(dd == null)
            {
                throw new Exception("Failed to get discovery document from " + conf?.OAuth2Authority);
            }
            return dd;
        }

        /// <summary>
        /// The token callvack
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> TokenCallbackAsync(
            string code = null,
            string state = null, 
            CancellationToken cancellationToken = default)
        {
            if (code == null) throw new ArgumentException("No code token supplied");

            var conf = GetOAuth2Conf();
            var auth = GetAuthority(conf);

            var redirectUri = await Invoker.GetUriAsync(o => o.TokenCallbackAsync(null, null, cancellationToken));

            var statems = new MemoryStream(Convert.FromBase64String(state));
            SerializationFactory.DeserializeFromStream(statems, out State statestruct);
            var token = await auth.GetCodeJwtToken(
                conf.OAuth2ClientId,
                conf.OAuth2ClientSecret,
                code,
                redirectUri.ToString(),
                null,
                cancellationToken);

            if(token == null || string.IsNullOrEmpty(token.AccessToken))
            {   
                throw new Exception("No token returned for supplied code.");
            }

            var callback = statestruct.Callback?.ToString() ?? "";
            if(callback.IndexOf('?') > 0)
            {
                callback = $"{callback}&access_token={token.AccessToken}";
            }
            else
            {
                callback = $"{callback}?access_token={token.AccessToken}";
            }

            var result = new FileContent();
            result.Location = callback;
            if (!string.IsNullOrEmpty(token.RefreshToken))
            {
                await SetRefreshTokenAsync(result, token.RefreshToken);
            }


            return result;
        }

        private async Task SetRefreshTokenAsync(FileContent result, string refreshToken)
        {
            refreshToken = refreshToken ?? "";
            var refreshUri = await Invoker.GetUriAsync(o => o.RefreshTokenAsync(null, CancellationToken.None));
            var path = refreshUri.AbsolutePath.ToString();
            path = string.Join("/", path.Split('/').Reverse().Skip(2).Reverse());
            var secure = string.Equals(refreshUri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) ? ";Secure" : "";

            //result.SetCookie = $"{RefreshTokenCookieName}={refreshToken};Path={path};Domain={refreshUri.Host};HttpOnly;SameSite=Strict;{secure}";
            result.SetCookie = $"{RefreshTokenCookieName}={refreshToken};Path={path};HttpOnly{secure}";
        }

        private async Task<FileContent> CreateContent(string resourcename, IDictionary<string, string> replace)
        {
            string content;
            resourcename = $"{nameof(SolidRpcOAuth2)}.{resourcename}.html";
            var a = GetType().Assembly;
            var an = a.GetManifestResourceNames().Single(n => n.EndsWith(resourcename));
            using (var s = a.GetManifestResourceStream(an))
            {
                using (var sr = new StreamReader(s))
                {
                    content = await sr.ReadToEndAsync();
                }
            }
            foreach(var kv in replace)
            {
                content = content.Replace($"{{{kv.Key}}}", kv.Value);
            }
            var enc = Encoding.UTF8;
            return new FileContent()
            {
                ContentType = "text/html",
                Content = new MemoryStream(enc.GetBytes(content)),
                CharSet = enc.HeaderName
            };
        }

        /// <summary>
        /// Returns a new access token for supplied token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<FileContent> RefreshTokenAsync(string accessToken = null, CancellationToken cancellation = default)
        {
            var conf = GetOAuth2Conf();
            var auth = GetAuthority(conf);

            // make sure that we get a refresh token as a cookie
            var refreshToken = GetRefreshToken();

            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            // use authority to refresh it
            var token = await auth.RefreshTokenAsync(conf.OAuth2ClientId, conf.OAuth2ClientSecret, refreshToken, cancellation);
            if(token == null)
            {
                return null;
            }

            if(!string.Equals(accessToken, "current", StringComparison.InvariantCultureIgnoreCase))
            {
                var origAccessTokenData = ParseAccessToken(accessToken);
                var newAccessTokenData = ParseAccessToken(token.AccessToken);
                if (newAccessTokenData?.Iss != origAccessTokenData?.Iss)
                {
                    throw new Exception("Issuer for access tokens does not match");
                }
                if (newAccessTokenData?.ClientId != origAccessTokenData?.ClientId)
                {
                    throw new Exception("ClientId for access tokens does not match");
                }
                if (newAccessTokenData?.Sub != origAccessTokenData?.Sub)
                {
                    throw new Exception("Subject for access tokens does not match");
                }
            }

            // send response
            var enc = Encoding.ASCII;
            var result = new FileContent()
            {
                CharSet = enc.HeaderName,
                ContentType = "text/plain",
                Content = new MemoryStream(enc.GetBytes(token.AccessToken))
            };
            if (!string.IsNullOrEmpty(token.RefreshToken))
            {
                await SetRefreshTokenAsync(result, token.RefreshToken);
            }

            return result;
        }

        private TokenData ParseAccessToken(string accessToken)
        {
            try
            {
                var parts = accessToken.Split('.');
                if(parts.Length < 2)
                {
                    return null;
                }
                var b64Payload = parts[1];
                var pad = b64Payload.Length % 4;
                switch(pad)
                {
                    case 0:
                        break;
                    case 2:
                        b64Payload += "==";
                        break;
                    case 3:
                        b64Payload += "=";
                        break;
                    default:
                        throw new Exception("Invalid JWT length");
                }

                SerializationFactory.DeserializeFromStream(new MemoryStream(Convert.FromBase64String(b64Payload)), out TokenData tokenData);
                return tokenData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private IEnumerable<NameValuePair> ParseCookies(IEnumerable<string> cookies)
        {
            if (cookies == null) return new NameValuePair[0];
            return cookies.Select(o => {
                var vals = o.Split(';').First().Split('=');
                return new NameValuePair() { Name = vals[0], Value = vals[1] };
            });
        }

        public async Task<FileContent> LogoutAsync(Uri callbackUri = null, string accessToken = null, CancellationToken cancellationToken = default)
        {
            CheckAllowedCallback(callbackUri);

            var conf = GetOAuth2Conf();
            var auth = GetAuthority(conf);

            // revoke refresh token
            var refreshToken = GetRefreshToken();
            if(!string.IsNullOrEmpty(refreshToken))
            {

                await auth.RevokeTokenAsync(conf.OAuth2ClientId, conf.OAuth2ClientSecret, refreshToken, cancellationToken);
            }

            // call end session
            var doc = await auth.GetDiscoveryDocumentAsync(cancellationToken);
            if(doc.EndSessionEndpoint != null)
            {
                var state = "";
                if(callbackUri != null)
                {
                    state = Convert.ToBase64String(Encoding.UTF8.GetBytes(callbackUri.ToString()));
                }
                var postLogoutRedirectUri = await Invoker.GetUriAsync(o => o.PostLogoutAsync(null, cancellationToken));
                var endSession = $"{doc.EndSessionEndpoint}?id_token_hint={accessToken}&post_logout_redirect_uri={HttpUtility.UrlEncode(postLogoutRedirectUri.ToString())}&state={state}";

                var result = new FileContent();
                result.Location = endSession;
                await SetRefreshTokenAsync(result, "");
                return result;
            }
            else
            {
                var result = new FileContent();
                result.Location = callbackUri.ToString();
                await SetRefreshTokenAsync(result, "");
                return result;
            }

        }

        private string GetRefreshToken()
        {
            var invocOpts = InvocationOptions.Current;
            if(invocOpts.TryGetValue(InvocationOptions.RequestHeaderInboundPrefix, "Cookie", out IEnumerable<string> cookies))
            {
                var tokens = ParseCookies(cookies)
                    .Where(o => o.Name == RefreshTokenCookieName);
                return tokens
                    .Select(o => o.Value)
                    .FirstOrDefault();
            }
            return null;
        }

        public async Task<FileContent> PostLogoutAsync(string state = null, CancellationToken cancellationToken = default)
        {

            var result = new FileContent();
            result.Location = Encoding.UTF8.GetString(Convert.FromBase64String(state));
            await SetRefreshTokenAsync(result, "");
            return result;
        }
    }
}
