namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcAbstractionsExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge>,SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="challenge"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task SetAcmeChallengeAsync(
                System.String challenge,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().SetAcmeChallengeAsync(challenge, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="cancellation"></param>
            [SolidRpc.Abstractions.OpenApi(Path="/.well-known/acme-challenge/{key}")]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetAcmeChallengeAsync(
                System.String key,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().GetAcmeChallengeAsync(key, cancellation);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcContentHandler>,SolidRpc.Abstractions.Services.ISolidRpcContentHandler {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcContentHandler> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            public System.Collections.Generic.IEnumerable<System.String> PathPrefixes {
                get {
                    throw new System.NotImplementedException();
                }
             }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetContent(
                System.String path,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetContent(path, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="redirects"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>> GetPathMappingsAsync(
                System.Boolean redirects,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetPathMappingsAsync(redirects, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.Byte[] resource,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetProtectedContentAsync(resource, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="fileName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.Byte[] resource,
                System.String fileName,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetProtectedContentAsync(resource, fileName, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcHostProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcHost>,SolidRpc.Abstractions.Services.ISolidRpcHost {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcHostProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcHost> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.Guid> GetHostId(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetHostId(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.SolidRpcHostInstance> GetHostInstance(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetHostInstance(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>> SyncHostsFromStore(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SyncHostsFromStore(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.SolidRpcHostInstance> CheckHost(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CheckHost(hostInstance, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.NameValuePair>> GetHostConfiguration(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetHostConfiguration(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task IsAlive(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().IsAlive(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.Uri> BaseAddress(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().BaseAddress(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.String>> AllowedCorsOrigins(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().AllowedCorsOrigins(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.String> DefaultTimezone(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().DefaultTimezone(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.Security(new [] {"SolidRpcHostPermission",})]
            public System.Threading.Tasks.Task<System.DateTimeOffset> ParseDateTime(
                System.String dateTime,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ParseDateTime(dateTime, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcHostStoreProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcHostStore>,SolidRpc.Abstractions.Services.ISolidRpcHostStore {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcHostStoreProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcHostStore> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddHostInstanceAsync(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().AddHostInstanceAsync(hostInstance, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hostInstance"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RemoveHostInstanceAsync(
                SolidRpc.Abstractions.Types.SolidRpcHostInstance hostInstance,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().RemoveHostInstanceAsync(hostInstance, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.SolidRpcHostInstance>> ListHostInstancesAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ListHostInstancesAsync(cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcMethodStore>,SolidRpc.Abstractions.Services.ISolidRpcMethodStore {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcMethodStore> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="takeCount"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>> GetHttpRequestAsync(
                System.Int32? takeCount,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().GetHttpRequestAsync(takeCount, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="solidRpcCallId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task RemoveHttpRequestAsync(
                System.String solidRpcCallId,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().RemoveHttpRequestAsync(solidRpcCallId, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="takeCount"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.HttpRequest>> GetHttpRequestsAsync(
                System.String sessionId,
                System.Int32? takeCount,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().GetHttpRequestsAsync(sessionId, takeCount, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="solidRpcCallId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task RemoveHttpRequestAsync(
                System.String sessionId,
                System.String solidRpcCallId,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().RemoveHttpRequestAsync(sessionId, solidRpcCallId, cancellation);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcOAuth2>,SolidRpc.Abstractions.Services.ISolidRpcOAuth2 {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcOAuth2> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callbackUri"></param>
            /// <param name="state"></param>
            /// <param name="scopes"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetAuthorizationCodeTokenAsync(
                System.Uri callbackUri,
                System.String state,
                System.Collections.Generic.IEnumerable<System.String> scopes,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetAuthorizationCodeTokenAsync(callbackUri, state, scopes, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="code"></param>
            /// <param name="state"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> TokenCallbackAsync(
                System.String code,
                System.String state,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().TokenCallbackAsync(code, state, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="accessToken"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> RefreshTokenAsync(
                System.String accessToken,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().RefreshTokenAsync(accessToken, cancellation);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callbackUri"></param>
            /// <param name="accessToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> LogoutAsync(
                System.Uri callbackUri,
                System.String accessToken,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().LogoutAsync(callbackUri, accessToken, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="state"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> PostLogoutAsync(
                System.String state,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().PostLogoutAsync(state, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcOidcProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcOidc>,SolidRpc.Abstractions.Services.ISolidRpcOidc {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcOidcProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcOidc> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Abstractions.OpenApi(Path="/.well-known/openid-configuration")]
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetDiscoveryDocumentAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.OpenIDKeys> GetKeysAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetKeysAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="grantType"></param>
            /// <param name="clientId"></param>
            /// <param name="clientSecret"></param>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="scope"></param>
            /// <param name="code"></param>
            /// <param name="redirectUri"></param>
            /// <param name="codeVerifier"></param>
            /// <param name="refreshToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.OAuth2.TokenResponse> GetTokenAsync(
                System.String grantType,
                System.String clientId,
                System.String clientSecret,
                System.String username,
                System.String password,
                System.Collections.Generic.IEnumerable<System.String> scope,
                System.String code,
                System.String redirectUri,
                System.String codeVerifier,
                System.String refreshToken,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetTokenAsync(grantType, clientId, clientSecret, username, password, scope, code, redirectUri, codeVerifier, refreshToken, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="responseType"></param>
            /// <param name="clientId"></param>
            /// <param name="redirectUri"></param>
            /// <param name="state"></param>
            /// <param name="responseMode"></param>
            /// <param name="nonce"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> AuthorizeAsync(
                System.Collections.Generic.IEnumerable<System.String> scope,
                System.String responseType,
                System.String clientId,
                System.String redirectUri,
                System.String state,
                System.String responseMode,
                System.String nonce,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().AuthorizeAsync(scope, responseType, clientId, redirectUri, state, responseMode, nonce, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="clientId"></param>
            /// <param name="clientSecret"></param>
            /// <param name="token"></param>
            /// <param name="tokenHint"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RevokeAsync(
                System.String clientId,
                System.String clientSecret,
                System.String token,
                System.String tokenHint,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().RevokeAsync(clientId, clientSecret, token, tokenHint, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="idTokenHint"></param>
            /// <param name="postLogoutRedirectUri"></param>
            /// <param name="state"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> EndSessionAsync(
                System.String idTokenHint,
                System.String postLogoutRedirectUri,
                System.String state,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().EndSessionAsync(idTokenHint, postLogoutRedirectUri, state, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent>,SolidRpc.Abstractions.Services.ISolidRpcProtectedContent {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resource"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> GetProtectedContentAsync(
                System.String resource,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetProtectedContentAsync(resource, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit>,SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="timeout"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken> GetRateLimitTokenAsync(
                System.String resourceName,
                System.TimeSpan timeout,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetRateLimitTokenAsync(resourceName, timeout, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="timeout"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.RateLimit.RateLimitToken> GetSingeltonTokenAsync(
                System.String resourceName,
                System.TimeSpan timeout,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSingeltonTokenAsync(resourceName, timeout, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="rateLimitToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ReturnRateLimitTokenAsync(
                SolidRpc.Abstractions.Types.RateLimit.RateLimitToken rateLimitToken,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ReturnRateLimitTokenAsync(rateLimitToken, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting>> GetRateLimitSettingsAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetRateLimitSettingsAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="setting"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateRateLimitSetting(
                SolidRpc.Abstractions.Types.RateLimit.RateLimitSetting setting,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpdateRateLimitSetting(setting, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator>,SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.Code.CodeNamespace> CreateCodeNamespace(
                System.String assemblyName,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateCodeNamespace(assemblyName, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeINpmGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.INpmGenerator>,SolidRpc.Abstractions.Services.Code.INpmGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeINpmGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.INpmGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyNames"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Abstractions.Types.Code.NpmPackage>> CreateNpmPackage(
                System.Collections.Generic.IEnumerable<System.String> assemblyNames,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateNpmPackage(assemblyNames, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Abstractions.Types.FileContent> CreateInitialZip(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateInitialZip(cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator>,SolidRpc.Abstractions.Services.Code.ITypescriptGenerator {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> CreateTypesTsForAssemblyAsync(
                System.String assemblyName,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateTypesTsForAssemblyAsync(assemblyName, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="codeNamespace"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> CreateTypesTsForCodeNamespaceAsync(
                SolidRpc.Abstractions.Types.Code.CodeNamespace codeNamespace,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateTypesTsForCodeNamespaceAsync(codeNamespace, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcAbstractions(
            Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcAcmeChallenge,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcAcmeChallengeProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcContentHandler,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcContentHandlerProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcHost,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcHostProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcHostStore,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcHostStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcMethodStore,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcMethodStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcOAuth2,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcOAuth2Proxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcOidc,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcOidcProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.ISolidRpcProtectedContent,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesISolidRpcProtectedContentProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.RateLimit.ISolidRpcRateLimit,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesRateLimitISolidRpcRateLimitProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.ICodeNamespaceGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeICodeNamespaceGeneratorProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.INpmGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeINpmGeneratorProxy>(configure);
            sc.SetupProxy<SolidRpc.Abstractions.Services.Code.ITypescriptGenerator,Microsoft.Extensions.DependencyInjection.SolidRpcAbstractionsExtensions.SolidRpcAbstractionsServicesCodeITypescriptGeneratorProxy>(configure);
            return sc;
        }
    
    }
}