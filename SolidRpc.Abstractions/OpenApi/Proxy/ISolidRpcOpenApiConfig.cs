using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{

    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public interface ISolidRpcOpenApiConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the open api specification to use. If not set the specification matching
        /// the method name or assembly name where the method is defined will be used.
        /// </summary>
        string OpenApiSpec { get; set; }

        /// <summary>
        /// The proxy transport type. If not explicitly configured the "Http" transport
        /// will be used for remote implementations and the "Local" transport for proxes that 
        /// are registerd in the IoC container.
        /// </summary>
        string ProxyTransportType { get; set; }
    }

    /// <summary>
    /// Extension methods to manipulate the settings
    /// </summary>
    public static class ISolidRpcOpenApiConfigExtensions
    {
        /// <summary>
        /// Sets the method address transformer on the transports
        /// </summary>
        /// <param name="config"></param>
        public static IEnumerable<ITransport> GetTransports(this ISolidRpcOpenApiConfig config)
        {
            var configs = config.InvocationConfiguration.GetAdviceConfigurations<ITransport>();
            if(!configs.Any())
            {
                var hc = config.InvocationConfiguration.ConfigureAdvice<IHttpTransport>();
                hc.MessagePriority = InvocationOptions.MessagePriorityNormal;

                configs = new[] { hc }; 
            }
            if (config.InvocationConfiguration.HasImplementation)
            {
                if(!configs.Any(o => o.GetTransportType() == "Local"))
                {
                    var lc = config.InvocationConfiguration.ConfigureAdvice<ILocalTransport>();
                    lc.MessagePriority = InvocationOptions.MessagePriorityNormal;
                    configs = configs.Union(new[] { lc });
                }
            }

            return configs;
        }



        /// <summary>
        /// Configures the forwarding transport used in the invoker.
        /// </summary>
        /// <param name="config"></param>
        public static TTransport ConfigureTransport<TTransport>(
            this ISolidRpcOpenApiConfig config) where TTransport : ITransport
        {
            return config.GetAdviceConfig<TTransport>();
        }

        /// <summary>
        /// Configures the ProxyTransport type.
        /// </summary>
        /// <typeparam name="TTransport"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TTransport SetProxyTransportType<TTransport>(
            this ISolidRpcOpenApiConfig config) where TTransport : ITransport
        {
            var transport = config.GetAdviceConfig<TTransport>();
            config.ProxyTransportType = transport.GetTransportType();
            return transport;
        }

        
        /// <summary>
        /// Configures the forwarding transport used in the invoker.
        /// </summary>
        /// <param name="config"></param>
        public static TFrom SetInvokerTransport<TFrom, TTo>(
            this ISolidRpcOpenApiConfig config) where TFrom:ITransport where TTo:ITransport
        {
            var c = config.ConfigureTransport<TFrom>();
            c.InvokerTransport = ITransportExtensions.GetTransportType(typeof(TTo));
            return c;
        }

        /// <summary>
        /// Sets the rate limit on this configuration.
        /// <code>
        /// var rateLimitConfig = config.GetAdviceConfig&lt;ISolidRpcRateLimitConfig&gt;();
        /// rateLimitConfig.ResourceName = resourceName;
        /// rateLimitConfig.Timeout = timeout;
        /// </code>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="resourceName"></param>
        /// <param name="timeout"></param>
        public static void SetRateLimit(this ISolidRpcOpenApiConfig config, string resourceName, TimeSpan timeout)
        {
            var rateLimitConfig = config.GetAdviceConfig<ISolidRpcRateLimitConfig>();
            rateLimitConfig.ResourceName = resourceName;
            rateLimitConfig.Timeout = timeout;
        }

        /// <summary>
        /// Sets the securitykry
        /// </summary>
        /// <param name="config"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetSecurityKey(this ISolidRpcOpenApiConfig config, string name, string value)
        {
            config.SetSecurityKey(new KeyValuePair<string, string>(name, value));
        }

        /// <summary>
        /// Sets the securitykry
        /// </summary>
        /// <param name="config"></param>
        /// <param name="value"></param>
        public static void SetSecurityKey(this ISolidRpcOpenApiConfig config, string value)
        {
            config.SetSecurityKey(new KeyValuePair<string, string>("SecurityKey", value));
        }

        /// <summary>
        /// Sets the securitykey
        /// </summary>
        /// <param name="config"></param>
        /// <param name="securityKey"></param>
        public static void SetSecurityKey(this ISolidRpcOpenApiConfig config, KeyValuePair<string, string> securityKey)
        {
            config.GetAdviceConfig<ISecurityKeyConfig>().SecurityKey = securityKey;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
        }

        /// <summary>
        /// Sets the OAuth2 security configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="authority"></param>
        public static ISecurityOAuth2Config SetOAuth2Security(this ISolidRpcOpenApiConfig config, string authority)
        {
            var adviceConfig = config.GetAdviceConfig<ISecurityOAuth2Config>();
            adviceConfig.OAuth2Authority = authority;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
            return adviceConfig;
        }

        /// <summary>
        /// Sets the OAuth2 security configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="authority"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scopes"></param>
        public static ISecurityOAuth2Config SetOAuth2ClientSecurity(this ISolidRpcOpenApiConfig config, string authority, string clientId, string clientSecret, params string[] scopes)
        {
            var adviceConfig = config.GetAdviceConfig<ISecurityOAuth2Config>();
            adviceConfig.OAuth2Authority = authority;
            adviceConfig.OAuth2ClientId = clientId;
            adviceConfig.OAuth2ClientSecret = clientSecret;
            adviceConfig.OAuth2Scopes = scopes;
            adviceConfig.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Client;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
            return adviceConfig;
        }

        /// <summary>
        /// Sets the OAuth2 security configuration
        /// </summary>
        /// <param name="config"></param>
        public static void SetOAuth2ProxySecurity(this ISolidRpcOpenApiConfig config)
        {
            var advice = config.GetAdviceConfig<ISecurityOAuth2Config>();
            advice.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Proxy;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
        }

        /// <summary>
        /// Sets the securitykey
        /// </summary>
        /// <param name="config"></param>
        public static void DisableSecurity(this ISolidRpcOpenApiConfig config)
        {
            config.GetAdviceConfig<ISecurityKeyConfig>().Enabled = false;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = false;
            config.GetAdviceConfig<ISolidAzureFunctionConfig>().HttpAuthLevel = "anonymous";
        }


    }
}
