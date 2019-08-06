using SolidProxy.Core.Configuration.Builder;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        private class DummyServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
        private static bool s_assembliesLoaded = false;

        /// <summary>
        /// Returns the service provider for specified service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TService GetSolidRpcServiceProvider<TService>(this IServiceCollection services) where TService:class
        {
            var service = services.SingleOrDefault(o => o.ServiceType == typeof(TService));
            if (service == null)
            {
                service = new ServiceDescriptor(typeof(TService), SolidRpcAbstractionProviderAttribute.CreateInstance<TService>());
                services.Add(service);
            }
            if (service.ImplementationInstance != null)
            {
                return (TService)service.ImplementationInstance;
            }
            if (service.ImplementationType != null)
            {
                var impl = Activator.CreateInstance(service.ImplementationType); // create before remove...
                services.Remove(service);
                service = new ServiceDescriptor(typeof(TService), impl);
                services.Add(service);
                return (TService)service.ImplementationInstance;
            }
            var proxied = services.SingleOrDefault(o => o.ServiceType == typeof(ISolidProxied<TService>));
            if(proxied != null)
            {
                if (proxied.ImplementationInstance != null)
                {
                    return ((ISolidProxied<TService>)proxied.ImplementationInstance).Service;
                }
                else if (proxied.ImplementationFactory != null)
                {
                    return ((ISolidProxied<TService>)proxied.ImplementationFactory(new DummyServiceProvider())).Service;
                }
                else
                {
                    throw new Exception("!!!");
                }
            }
            throw new Exception($"Cannot find singleton service for {typeof(TService)}.");
        }

        /// <summary>
        /// Registers a singleton service provider.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterSingletonService<TService>(this IServiceCollection services) where TService : class
        {
            if (!services.Any(o => typeof(TService) == o.ServiceType))
            {
                services.AddSingleton(typeof(TService), SolidRpcAbstractionProviderAttribute.GetImplemenationType<TService>());
            }
            return services;
        }

        /// <summary>
        /// Adds the singleton services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSingletonServices(this IServiceCollection services)
        {
            InitAssemblies();
            services.RegisterSingletonService<IOpenApiParser>();
            services.RegisterSingletonService<IMethodInvoker>();
            services.RegisterSingletonService<IMethodBinderStore>();
            services.RegisterSingletonService<IBaseUriTransformer>();
            return services;
        }

        private static void InitAssemblies()
        {
            if (s_assembliesLoaded) return;
            LoadAssembly("SolidRpc.OpenApi.Model");
            LoadAssembly("SolidRpc.OpenApi.Binder");
            LoadAssembly("SolidRpc.OpenApi.AspNetCore", false);
            LoadAssembly("SolidRpc.OpenApi.AzFunctions", false);
            LoadAssembly("SolidRpc.OpenApi.AzFunctionsV1Extension", false);
            LoadAssembly("SolidRpc.OpenApi.AzFunctionsV2Extension", false);
            s_assembliesLoaded = true;
        }

        private static void LoadAssembly(string assemblyName, bool required = true)
        {
            try
            {
                Assembly.Load(assemblyName);
            }
            catch(Exception)
            {
                if (required) throw;
            }
        }

        /// <summary>
        /// Returns the static content provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ISolidRpcStaticContent GetSolidRpcStaticContent(this IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            return services.GetSolidRpcServiceProvider<ISolidRpcStaticContent>();
        }

        /// <summary>
        /// Returns the static content provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IOpenApiParser GetSolidRpcOpenApiParser(this IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            return services.GetSolidRpcServiceProvider<IOpenApiParser>();
        }


        /// <summary>
        /// Configures all the interfaces in supplied assembly
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementationAssembly"></param>
        /// <param name="baseUriTransformer"></param>
        public static IServiceCollection AddSolidRpcBindings(this IServiceCollection sc, Assembly interfaceAssembly, Assembly implementationAssembly = null, BaseUriTransformer baseUriTransformer = null)
        {
            // use the interface as implementation
            if (implementationAssembly == null)
            {
                implementationAssembly = interfaceAssembly;
            }

            foreach (var t in interfaceAssembly.GetTypes())
            {
                if (!t.IsInterface)
                {
                    continue;
                }
                Type impl = null;
                if (implementationAssembly != null)
                {
                    impl = implementationAssembly.GetTypes()
                        .Where(o => t.IsAssignableFrom(o))
                        .Where(o => o != t)
                        .SingleOrDefault();
                }
                sc.AddSolidRpcBindings(t, impl, null, baseUriTransformer);
            }
            return sc;
        }

        /// <summary>
        /// Configures the supplied type so that it is exposed in the binder.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaze"></param>
        /// <param name="impl"></param>
        /// <param name="openApiConfiguration"></param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        public static IEnumerable<ISolidMethodConfigurationBuilder> AddSolidRpcBindings(this IServiceCollection sc, Type interfaze, Type impl = null, string openApiConfiguration = null, BaseUriTransformer baseUriTransformer = null)
        {
            //
            // make sure that the type is registered
            //
            if (impl != null && !sc.Any(o => o.ServiceType == interfaze))
            {
                sc.AddTransient(interfaze, impl);
            }

            if (!sc.Any(o => o.ServiceType == interfaze))
            {
                return new ISolidMethodConfigurationBuilder[0];
            }

            return interfaze.GetMethods()
                .Select(m => sc.AddSolidRpcBinding(m, openApiConfiguration, baseUriTransformer))
                .ToList();
        }

        /// <summary>
        /// Configures the suppllied method so that it is exposed through the .net core http binding.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="mi"></param>
        /// <param name="openApiConfiguration">The open api configuration to use - may be null to use the embedded api config.</param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        public static ISolidMethodConfigurationBuilder AddSolidRpcBinding(this IServiceCollection sc, MethodInfo mi, string openApiConfiguration = null, BaseUriTransformer baseUriTransformer = null)
        {
            //
            // make sure that the singleton services are registered
            //
            sc.AddSolidRpcSingletonServices();

            //
            // configure method
            //
            var mc = sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(mi.DeclaringType.Assembly)
                .ConfigureInterface(mi.DeclaringType)
                .ConfigureMethod(mi);

            var openApiProxyConfig = mc.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            openApiProxyConfig.OpenApiConfiguration = openApiConfiguration;
            openApiProxyConfig.BaseUriTransformer = baseUriTransformer;

            //
            // make sure that the implementation is wrapped in a proxy by adding the invocation advice.
            // 
            var serviceRegistration = sc.FirstOrDefault(o => o.ServiceType == mi.DeclaringType);
            if ((serviceRegistration?.ImplementationType?.IsClass ?? false) || serviceRegistration?.ImplementationInstance != null || serviceRegistration.ImplementationFactory != null)
            {
                mc.AddAdvice(typeof(SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice<,,>));
            }

            return mc;
        }
    }
}
