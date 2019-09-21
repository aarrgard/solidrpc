using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.NpmGenerator.InternalServices;
using SolidRpc.NpmGenerator.Services;
using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods or the service collections
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the npm generator.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcNpmGenerator(this IServiceCollection services, MethodAddressTransformer baseUriTransformer = null)
        {
            services.AddTransient<ICodeNamespaceGenerator, CodeNamespaceGenerator>();
            services.AddTransient<ITypescriptGenerator, TypeScriptGenerator>();
            if(!services.Any(o => o.ServiceType == typeof(ICodeDocRepository)))
            {
                services.AddSingleton<ICodeDocRepository, CodeDocRepository>();
            }
            services.AddNodeServices();
            services.AddHttpClient();
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(INpmGenerator));
            var strOpenApiSpec = openApiSpec.WriteAsJsonString();

            services.AddSolidRpcBindings(typeof(INpmGenerator), typeof(NpmGenerator), strOpenApiSpec, baseUriTransformer);
            return services;
        }
    }
}
