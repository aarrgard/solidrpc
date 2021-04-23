using SolidProxy.Core.Configuration;
using System;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// Configures the OAuth2 security
    /// </summary>
    public interface ISecurityOAuth2Config : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The authority uri. Add .well-known/openid-configuration to fetch the config. 
        /// </summary>
        string OAuth2Authority { get; set; }

        /// <summary>
        /// The client id.
        /// </summary>
        string OAuth2ClientId { get; set; }

        /// <summary>
        /// The client secret - will be used to authenticate the client if client credentials are used
        /// </summary>
        string OAuth2ClientSecret { get; set; }

        /// <summary>
        /// The scopes that are required for this invocation - default is "SolidRpc"
        /// </summary>
        IEnumerable<string> OAuth2Scopes { get; set; }

        /// <summary>
        /// Specifies if unauthorized identities should be redirected to the authorization endpoint instead of 
        /// raising UnauthorizedException.
        /// </summary>
        bool RedirectUnauthorizedIdentity { get; set; }

        /// <summary>
        /// Which authorization should be used when invoking proxies.
        /// </summary>
        OAuthProxyInvocationPrincipal OAuthProxyInvocationPrincipal { get; set; }
    }
}
