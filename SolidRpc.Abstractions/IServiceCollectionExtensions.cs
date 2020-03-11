using Microsoft.Extensions.Configuration;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service if type is missing in the collection.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonIfMissing<T1, T2>(this IServiceCollection services) where T1 : class where T2 : class, T1
        {
            if (!services.Any(o => o.ServiceType == typeof(T1)))
            {
                return services.AddSingleton<T1, T2>();
            }
            return services;
        }

        /// <summary>
        /// Adds the factory if it is missing.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="services"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonIfMissing<T1>(this IServiceCollection services, Func<IServiceProvider, T1> factory) where T1 : class
        {
            if (!services.Any(o => o.ServiceType == typeof(T1)))
            {
                return services.AddSingleton(factory);
            }
            return services;
        }

        private class DummyServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
        private static bool s_assembliesLoaded = false;

        /// <summary>
        /// Adds the rpc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcServices(
            this IServiceCollection services, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            var methods = typeof(ISolidRpcContentHandler).GetMethods()
                .Where(o => o.Name == nameof(ISolidRpcContentHandler.GetContent))
                .Union(typeof(ISolidRpcHost).GetMethods());

            var openApiParser = services.GetSolidRpcOpenApiParser();
            var solidRpcHostSpec = openApiParser.CreateSpecification(methods.ToArray()).WriteAsJsonString();
            methods.ToList().ForEach(m =>
            {
                services.AddSolidRpcBinding(m, (c) => {
                    
                    c.OpenApiSpec = solidRpcHostSpec;

                    //
                    // remove securitykey for the content handler
                    //
                    var method = c.Methods.Single();
                    if(method.DeclaringType == typeof(ISolidRpcContentHandler))
                    {
                        c.SecurityKey = null;
                    }
                    return configurator?.Invoke(c) ?? false;
                });
            });

            return services;
        }

        /// <summary>
        /// Returns the service provider for specified service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public static TService GetSolidRpcServiceProvider<TService>(this IServiceCollection services, bool mustExist = true) where TService:class
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
            if(mustExist)
            {
                throw new Exception($"Cannot find singleton service for {typeof(TService)}.");
            }
            return null;
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
                var implType = SolidRpcAbstractionProviderAttribute.GetImplemenationType<TService>();
                if(implType.GetConstructor(Type.EmptyTypes) == null)
                {
                    services.AddSingleton(implType, implType);
                    services.AddSingleton(typeof(TService), _ => _.GetService(implType));
                }
                else
                {
                    var impl = Activator.CreateInstance(implType);
                    services.AddSingleton(implType, impl);
                    services.AddSingleton(typeof(TService), impl);
                }
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
            services.RegisterSingletonService<IOpenApiSpecResolver>();
            services.RegisterSingletonService<IMethodInvoker>();
            services.RegisterSingletonService<IMethodBinderStore>();
            services.RegisterSingletonService<IMethodAddressTransformer>();
            services.RegisterSingletonService<ISolidRpcContentStore>();
            services.RegisterSingletonService<ISolidRpcContentHandler>();
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
        public static ISolidRpcContentStore GetSolidRpcContentStore(this IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            return services.GetSolidRpcServiceProvider<ISolidRpcContentStore>();
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
        /// <param name="configurator"></param>
        public static IServiceCollection AddSolidRpcBindings(
            this IServiceCollection sc, Assembly interfaceAssembly, 
            Assembly implementationAssembly = null, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
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
                if(impl == null)
                {
                    continue;
                }
                sc.AddSolidRpcBindings(t, impl, configurator);
            }
            return sc;
        }

        /// <summary>
        /// Configures the supplied type so that it is exposed in the binder.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaze"></param>
        /// <param name="impl"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IEnumerable<ISolidRpcOpenApiConfig> AddSolidRpcBindings(
            this IServiceCollection sc, 
            Type interfaze, 
            Type impl = null, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            //
            // make sure that the type is registered
            //
            if (!sc.Any(o => o.ServiceType == interfaze))
            {
                sc.AddTransient(interfaze, impl ?? interfaze);
            }

            if (!sc.Any(o => o.ServiceType == interfaze))
            {
                return new ISolidRpcOpenApiConfig[0];
            }

            return interfaze.GetMethods()
                .Select(m => sc.AddSolidRpcBinding(m, configurator))
                .ToList();
        }

        /// <summary>
        /// Configures the supplied type so that it is exposed in the binder.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="impl"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IEnumerable<ISolidRpcOpenApiConfig> AddSolidRpcBindings<T>(
            this IServiceCollection sc, T impl, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null) where T:class 
        {
            if(impl != null)
            {
                sc.AddSingleton<T>(impl);
            }
            else
            {
                sc.AddTransient<T, T>();
            }
            return typeof(T).GetMethods()
                .Select(m => sc.AddSolidRpcBinding(m, configurator))
                .ToList();
        }

        /// <summary>
        /// Configures the suppllied method so that it is exposed through the .net core http binding.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="mi"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static ISolidRpcOpenApiConfig AddSolidRpcBinding(
            this IServiceCollection sc, 
            MethodInfo mi, 
            Func<ISolidRpcOpenApiConfig, bool> configurator)
        {
            //
            // make sure that the singleton services are registered
            //
            sc.AddSolidRpcSingletonServices();

            //
            // check that we have an implementation - register it if that is the case
            //
            var serviceRegistration = sc.FirstOrDefault(o => o.ServiceType == mi.DeclaringType);
            if (serviceRegistration == null)
            {
                var implType = SolidRpcAbstractionProviderAttribute.GetImplemenationType(mi.DeclaringType);
                if (implType != null)
                {
                    serviceRegistration = new ServiceDescriptor(mi.DeclaringType, implType, ServiceLifetime.Singleton);
                    sc.Add(serviceRegistration);
                }
            }

            //
            // configure method
            //
            var mc = sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(mi.DeclaringType.Assembly)
                .ConfigureInterface(mi.DeclaringType)
                .ConfigureMethod(mi);

            var openApiProxyConfig = mc.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            SetSecurityKey(openApiProxyConfig, sc.GetSolidRpcServiceProvider<IConfiguration>(false));
            var enabled = configurator?.Invoke(openApiProxyConfig) ?? true;
            if(openApiProxyConfig.Enabled != mc.Enabled)
            {
                openApiProxyConfig.Enabled = enabled;
            }

            //
            // make sure that we apply security.
            // we cannot do this in the "SetSecurityKey" section since the
            // custom configurator may change/remove the key.(ISolidRpcContentHandler)
            //
            var secKey = openApiProxyConfig.SecurityKey;
            if (secKey != null)
            {
                var key = secKey.Value.Key;
                var value = secKey.Value.Value;
                mc.AddPreInvocationCallback(i =>
                {
                    var callKey = i.GetValue<string>($"HTTP_{key}");
                    if(!value.Equals(callKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new UnauthorizedException();
                    }
                    return Task.CompletedTask;
                });
            }

            return openApiProxyConfig;
        }

        private static void SetSecurityKey(ISolidRpcOpenApiConfig openApiProxyConfig, IConfiguration configuration)
        {
            if (configuration == null) return;
            var method = openApiProxyConfig.Methods.Single();
            var assemblyName = method.DeclaringType.Assembly.GetName().Name;
            var methodName = $"{method.DeclaringType.FullName}.{method.Name}";
            if(methodName.StartsWith($"{assemblyName}."))
            {
                methodName = methodName.Substring(assemblyName.Length+1);
            }
            var key1 = "SolidRpcSecurityKey";
            var key2 = $"{key1}.{assemblyName}";
            var key3 = $"{key2}.{methodName}";
            var val = configuration[key3];
            if (val != null)
            {
                openApiProxyConfig.SecurityKey = new KeyValuePair<string, string>(key3, val);
                return;
            }
            val = configuration[key2];
            if (val != null)
            {
                openApiProxyConfig.SecurityKey = new KeyValuePair<string, string>(key2, val);
                return;
            }
            val = configuration[key1];
            if (val != null)
            {
                openApiProxyConfig.SecurityKey = new KeyValuePair<string, string>(key1, val);
                return;
            }
        }
    }
}
