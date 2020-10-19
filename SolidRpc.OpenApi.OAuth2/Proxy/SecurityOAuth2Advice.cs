﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.OAuth2.InternalServices;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SecurityOAuth2Advice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SecurityPathClaimAdvice<,,>) };
        public static IEnumerable<Type> AfterAdvices = new Type[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authorityFactory"></param>
        public SecurityOAuth2Advice(
            ILogger<SolidRpcRateLimitAdvice<TObject, TMethod, TAdvice>> logger,
            IAuthorityFactory authorityFactory)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            AuthorityFactory = authorityFactory;
        }
        private ILogger Logger { get; }
        private IAuthorityFactory AuthorityFactory { get; }
        private bool RemoteCall { get; set; }
        private IAuthority Authority { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private OAuthProxyInvocationPrincipal ProxyInvocationPrincipal { get; set; }

        /// <summary>
        /// Configure the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISecurityOAuth2Config config)
        {
            if(config.OAuth2Authority == null) 
            {
                return false;
            }
            RemoteCall = !config.InvocationConfiguration.HasImplementation;
            Authority = AuthorityFactory.GetAuthority(config.OAuth2Authority);
            ClientId = config.OAuth2ClientId;
            ClientSecret = config.OAuth2ClientSecret;
            ProxyInvocationPrincipal = config.OAuthProxyInvocationPrincipal;
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

            if(RemoteCall)
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
            var authHeader = invocation.GetValue<StringValues>($"http_authorization").ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return;
            }
            if(!authHeader.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            var jwt = authHeader.Substring("bearer ".Length);
            var authorization = invocation.ServiceProvider.GetRequiredService<ISolidRpcAuthorization>();
            authorization.CurrentPrincipal = await Authority.GetPrincipalAsync(jwt, invocation.CancellationToken);
        }

        private async Task HandleRemoteCall(ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.None)
            {
                return;
            }
            if (ProxyInvocationPrincipal == OAuthProxyInvocationPrincipal.Client)
            {
                var jwt = await Authority.GetClientJwtAsync(ClientId, ClientSecret);
                invocation.SetValue<StringValues>("http_authorization", $"bearer {jwt}");
                return;
            }
            throw new NotImplementedException();
        }
    }
}
