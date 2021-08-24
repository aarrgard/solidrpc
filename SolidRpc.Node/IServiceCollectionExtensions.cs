using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the service collections
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the node service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcNode(
            this IServiceCollection services, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            var strNsOpenApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(INodeService)).WriteAsJsonString();
            services.AddSolidRpcBindings(
                  typeof(INodeService),
                  typeof(NodeService),
                  (c) =>
                  {
                      c.OpenApiSpec = strNsOpenApiSpec;
                      return configurator?.Invoke(c) ?? true;
                  });
            /*
            var strNgOpenApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(INpmGenerator)).WriteAsJsonString();
            services.AddSolidRpcBindings(
                  typeof(INpmGenerator),
                  typeof(NpmGenerator),
                  (c) =>
                  {
                      c.OpenApiSpec = strNgOpenApiSpec;
                      return configurator?.Invoke(c) ?? true;
                  });
            */
            return services;
        }
    }
}
