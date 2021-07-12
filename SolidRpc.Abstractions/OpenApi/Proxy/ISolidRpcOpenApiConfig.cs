﻿using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.OpenApi.Transport.Impl;
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

        /// <summary>
        /// The configured http transport
        /// </summary>
        IHttpTransport HttpTransport { get; set; }

        /// <summary>
        /// The configured http transport
        /// </summary>
        IQueueTransport QueueTransport { get; set; }
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
            bool transportFound = false;
            var httpTransport = config.HttpTransport;
            if (httpTransport != null)
            {
                transportFound = true;
                yield return httpTransport;
            }
            var queueTransport = config.QueueTransport;
            if (queueTransport != null)
            {
                transportFound = true;
                yield return queueTransport;
            }
            if(config.InvocationConfiguration.HasImplementation)
            {
                yield return new LocalTransport(InvocationStrategy.Invoke);
            }
            if(!transportFound)
            {
                config.HttpTransport = new HttpTransport(InvocationStrategy.Invoke, null);
                yield return config.HttpTransport;
            }
        }

        /// <summary>
        /// Activates the http transport
        /// </summary>
        /// <param name="config"></param>
        /// <param name="invocationStrategy"></param>
        public static void SetHttpTransport(this ISolidRpcOpenApiConfig config, InvocationStrategy? invocationStrategy = null)
        {
            var httpTransport = config.HttpTransport;
            if (httpTransport == null)
            {
                httpTransport = new HttpTransport(invocationStrategy ?? InvocationStrategy.Invoke, null);
            }
            else if(invocationStrategy != null)
            {
                httpTransport = httpTransport.SetInvocationStrategy(invocationStrategy.Value);
            }
            config.HttpTransport = httpTransport;
        }
        /// <summary>
        /// Sets the method address transformer on the transports
        /// </summary>
        /// <param name="config"></param>
        /// <param name="methodAddressTransformer"></param>
        public static void SetMethodAddressTransformer(this ISolidRpcOpenApiConfig config, MethodAddressTransformer methodAddressTransformer)
        {
            var httpTransport = config.HttpTransport;
            if (httpTransport == null)
            {
                httpTransport = new HttpTransport(InvocationStrategy.Invoke, methodAddressTransformer);
            }
            else
            {
                httpTransport = httpTransport.SetMethodAddressTransformer(methodAddressTransformer);
            }
            config.HttpTransport = httpTransport;
        }

        /// <summary>
        /// Sets the queue transport type
        /// </summary>
        /// <param name="config"></param>
        /// <param name="invocationStrategy"></param>
        /// <param name="connectionName"></param>
        /// <param name="queueName"></param>
        public static void SetQueueTransport<T>(this ISolidRpcOpenApiConfig config, InvocationStrategy? invocationStrategy = null, string connectionName = null, string queueName = null) where T : IHandler
        {
            string queueType = typeof(T).FullName.Split('.').Last();
            if(queueType.EndsWith("Handler"))
            {
                queueType = queueType.Substring(0, queueType.Length - "Handler".Length);
            }
            var queueTransport = config.QueueTransport;
            if (queueTransport == null)
            {
                queueTransport = new QueueTransport(invocationStrategy ?? InvocationStrategy.Invoke, null, null, queueType, null);
            }
            if (!string.IsNullOrEmpty(connectionName))
            {
                queueTransport = queueTransport.SetConnectionName(connectionName);
            }
            if (!string.IsNullOrEmpty(queueName))
            {
                queueTransport = queueTransport.SetQueueName(queueName);
            }
            config.QueueTransport = queueTransport;
        }

        /// <summary>
        /// Sets the queue transport connection name
        /// </summary>
        /// <param name="config"></param>
        /// <param name="connectionName"></param>
        public static void SetQueueTransportConnectionName(this ISolidRpcOpenApiConfig config, string connectionName)
        {
            var queueTransport = config.QueueTransport;
            if (queueTransport == null)
            {
                queueTransport = new QueueTransport(InvocationStrategy.Invoke, null, connectionName, null, null);
            }
            else
            {
                queueTransport = queueTransport.SetConnectionName(connectionName);
            }
            config.QueueTransport = queueTransport;
        }

        /// <summary>
        /// Sets the queue transport inbound handler
        /// </summary>
        /// <param name="config"></param>
        /// <param name="inboundHandler"></param>
        public static void SetQueueTransportInboundHandler(this ISolidRpcOpenApiConfig config, string inboundHandler)
        {
            var queueTransport = config.QueueTransport;
            if (queueTransport == null)
            {
                queueTransport = new QueueTransport(InvocationStrategy.Invoke, null, null, null, inboundHandler);
            }
            else
            {
                queueTransport = queueTransport.SetInboundHandler(inboundHandler);
            }
            config.QueueTransport = queueTransport;
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
        public static void SetOAuth2Security(this ISolidRpcOpenApiConfig config, string authority)
        {
            var advice = config.GetAdviceConfig<ISecurityOAuth2Config>();
            advice.OAuth2Authority = authority;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
        }

        /// <summary>
        /// Sets the OAuth2 security configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="authority"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scopes"></param>
        public static void SetOAuth2ClientSecurity(this ISolidRpcOpenApiConfig config, string authority, string clientId, string clientSecret, params string[] scopes)
        {
            var advice = config.GetAdviceConfig<ISecurityOAuth2Config>();
            advice.OAuth2Authority = authority;
            advice.OAuth2ClientId = clientId;
            advice.OAuth2ClientSecret = clientSecret;
            advice.OAuth2Scopes = scopes;
            advice.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Client;
            config.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
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
