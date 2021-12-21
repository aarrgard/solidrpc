//using SolidRpc.Abstractions.OpenApi.Proxy;
//using SolidRpc.Test.Vitec.Impl;
//using SolidRpc.Test.Vitec.Services;
//using System;

//namespace Microsoft.Extensions.DependencyInjection
//{
//    /// <summary>
//    /// Extension methods or the service collections
//    /// </summary>
//    public static class IServiceCollectionExtensions
//    {
//        /// <summary>
//        /// Adds the swagger UI to the service collection.
//        /// </summary>
//        /// <param name="services"></param>
//        /// <param name="baseUriTransformer"></param>
//        /// <returns></returns>
//        public static IServiceCollection AddVitec(this IServiceCollection services, Func<ISolidRpcOpenApiConfig, bool> configurator = null)
//        {
//            services.AddSolidRpcBindings(typeof(IEstate).Assembly, typeof(EstateImpl).Assembly, configurator);
//            return services;
//        }

//        /// <summary>
//        /// Adds the swagger UI to the service collection.
//        /// </summary>
//        /// <param name="services"></param>
//        /// <param name="baseUriTransformer"></param>
//        /// <returns></returns>
//        public static IServiceCollection AddVitecBackendServiceProvider(this IServiceCollection services)
//        {
//            services.AddSingleton<IVitecBackendServiceProvider, VitecBackendServiceProvider>();
//            return services;
//        }


//    }
//}
