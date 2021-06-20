﻿using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.NpmGenerator.Services;
using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using System;
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
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcNpmGenerator(
            this IServiceCollection services, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            services.AddSolidRpcNode(configurator);
            services.AddHttpClient();
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(INpmGenerator));
            var strOpenApiSpec = openApiSpec.WriteAsJsonString();

            services.AddSolidRpcBindings(
                typeof(INpmGenerator),
                typeof(NpmGenerator),
                (c) =>
                {
                    c.OpenApiSpec = strOpenApiSpec;
                    return configurator?.Invoke(c) ?? true;
                });
            return services;
        }
    }
}
