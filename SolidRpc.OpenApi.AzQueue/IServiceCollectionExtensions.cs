using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Services;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the az table queue services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcAzTableQueue(this IServiceCollection services, string connectionName, string inboundHandler, Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            services.AddSolidRpcBindings(typeof(IAzTableQueue), typeof(AzTableQueue), conf =>
            {
                conf.OpenApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IAzTableQueue)).WriteAsJsonString();

                if (conf.Methods.First().Name == nameof(IAzTableQueue.ProcessMessageAsync))
                {
                    conf.SetProxyTransportType<IAzQueueTransport>();
                    conf.SetInvokerTransport<IHttpTransport, IAzQueueTransport>();
                    conf.ConfigureTransport<IAzQueueTransport>()
                        .SetConnectionName(connectionName)
                        .SetInboundHandler(inboundHandler);
                }
                
                if (conf.Methods.First().Name == nameof(IAzTableQueue.ProcessTestMessage))
                {
                    conf.SetInvokerTransport<IHttpTransport, IAzQueueTransport>();
                    conf.ConfigureTransport<IAzTableTransport>().SetConnectionName(connectionName);
                }

                return configurator?.Invoke(conf) ?? true;
            });
            return services;
        }
    }
}
