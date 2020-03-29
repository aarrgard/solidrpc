using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.OpenApi.Transport.Impl;
using System.Collections.Generic;

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
        /// If this key is set it needs to be specified in the invocation
        /// properties in order for the invocation to be authorized on the server side.
        /// If the key is present on the client side it is added to call so that
        /// the invocation is authorized.
        /// </summary>
        KeyValuePair<string, string>? SecurityKey { get; set; }

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
            if(!transportFound)
            {
                config.HttpTransport = new HttpTransport(null, null);
                yield return config.HttpTransport;
            }
        }

        /// <summary>
        /// Activates the http transport
        /// </summary>
        /// <param name="config"></param>
        public static void SetHttpTransport(this ISolidRpcOpenApiConfig config)
        {
            var httpTransport = config.HttpTransport;
            if (httpTransport == null)
            {
                httpTransport = new HttpTransport(null, null);
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
                httpTransport = new HttpTransport(methodAddressTransformer, null);
            }
            else
            {
                httpTransport = httpTransport.SetMethodAddressTransformer(methodAddressTransformer);
            }
            config.HttpTransport = httpTransport;
        }

        /// <summary>
        /// Sets the queue transport to "AzSvcBus"
        /// </summary>
        /// <param name="config"></param>
        public static void SetAzSvcBusTransport(this ISolidRpcOpenApiConfig config)
        {
            config.SetQueueTransport("AzSvcBus");
        }

        /// <summary>
        /// Sets the queue transport to "AzQueue"
        /// </summary>
        /// <param name="config"></param>
        public static void SetAzQueueTransport(this ISolidRpcOpenApiConfig config)
        {
            config.SetQueueTransport("AzQueue");
        }

        /// <summary>
        /// Sets the queue transport type
        /// </summary>
        /// <param name="config"></param>
        /// <param name="queueType"></param>
        public static void SetQueueTransport(this ISolidRpcOpenApiConfig config, string queueType)
        {
            var queueTransport = config.QueueTransport;
            if (queueTransport == null)
            {
                queueTransport = new QueueTransport(null, null, queueType, null);
            }
            else
            {
                queueTransport = queueTransport.SetQueueType(queueType);
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
                queueTransport = new QueueTransport(null, connectionName, null, null);
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
                queueTransport = new QueueTransport(null, null, null, inboundHandler);
            }
            else
            {
                queueTransport = queueTransport.SetInboundHandler(inboundHandler);
            }
            config.QueueTransport = queueTransport;
        }
    }
}
