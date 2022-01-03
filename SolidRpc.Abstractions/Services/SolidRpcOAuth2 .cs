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
            if(!(scopes ?? new string[0]).Any())
            {
                scopes = new string[] { "openid", "solidrpc" };
            }
            var host = callbackUri.Host.Split(':').First();
            if (!AllowedHosts.Contains(host))
            {
                throw new ArgumentException($"Host not allowed({host}). Allowed hosts are {string.Join(",", AllowedHosts)}");
            }

            var conf = GetOAuth2Conf();
            var doc = await GetDiscoveryDocumentAsync(conf, cancellationToken);

            var statems = new MemoryStream();
            SerializationFactory.SerializeToStream(statems, new State()
            {
                ClientState = state,
                Callback = callbackUri
            });

            var redirectUri = await Invoker.GetUriAsync(o => o.TokenCallbackAsync(null, null, cancellationToken));

            return await CreateContent(nameof(GetAuthorizationCodeTokenAsync), new Dictionary<string, string>()
            {
                { "authorizationEndpoint", $"{doc.AuthorizationEndpoint}"},
                { "client_id", HttpUtility.UrlEncode(conf.OAuth2ClientId)},
                { "state", HttpUtility.UrlEncode(Convert.ToBase64String(statems.ToArray()))},
                { "scope", HttpUtility.UrlEncode(string.Join(" ", scopes)) },
                { "redirect_uri", HttpUtility.UrlEncode(redirectUri.ToString())}
            });
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
            return await GetAuthority(conf).GetDiscoveryDocumentAsync(cancellationToken);
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

            var callback = statestruct.Callback?.ToString() ?? "";
            if(callback.IndexOf('?') > 0)
            {
                callback = $"{callback}&access_token={token}";
            }
            else
            {
                callback = $"{callback}?access_token={token}";
            }

            return await CreateContent(nameof(TokenCallbackAsync), new Dictionary<string, string>()
            {
                { "accessToken", token},
                { "callback", callback }
           }); ;
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
    }
}
