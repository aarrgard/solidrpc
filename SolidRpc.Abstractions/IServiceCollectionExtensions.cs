using Microsoft.Extensions.Configuration;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Services.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the solidrpc logic
    /// </summary>
    public static class IServiceCollectionExtensions
    {

        private class DummyServiceProvider : IServiceProvider
        {
            public DummyServiceProvider(IServiceCollection services)
            {
                Services = services;
            }

            private IServiceCollection Services { get; }

            public object GetService(Type serviceType)
            {
                return GetService(Services, serviceType, false);
            }

            public static object GetService(IServiceCollection services, Type serviceType, bool mustExist)
            {
                if(serviceType == typeof(IServiceProvider))
                {
                    return new DummyServiceProvider(services);
                }
                var serviceProspects = services.Where(o => o.ServiceType == serviceType);
                ServiceDescriptor service;
                if (serviceProspects.Count() != 1)
                {
                    service = serviceProspects.FirstOrDefault();
                }
                else
                {
                    service = serviceProspects.FirstOrDefault();
                }
                if (service == null)
                {
                    if (mustExist)
                    {
                        throw new Exception($"Cannot find singleton service for {serviceType}.");
                    }
                    return DefaultValue(serviceType);
                }
                if (service.ImplementationInstance != null)
                {
                    return service.ImplementationInstance;
                }
                if (service.ImplementationType != null)
                {
                    var args = service.ImplementationType.GetConstructors().First()
                        .GetParameters().Select(o => o.ParameterType)
                        .Select(o => services.GetSolidRpcService(o, true))
                        .ToArray();
                    var impl = Activator.CreateInstance(service.ImplementationType, args); // create before remove...
                    services.Remove(service);
                    service = new ServiceDescriptor(serviceType, impl);
                    services.Add(service);
                    return service.ImplementationInstance;
                }
                if (service.ImplementationFactory != null)
                {
                    return service.ImplementationFactory(null);
                }
                var proxiedType = typeof(ISolidProxied<>).MakeGenericType(serviceType);
                var proxied = services.SingleOrDefault(o => o.ServiceType == proxiedType);
                if (proxied != null)
                {
                    if (proxied.ImplementationInstance != null)
                    {
                        throw new Exception();
                        //return ((ISolidProxied)proxied.ImplementationInstance).Service;
                    }
                    else if (proxied.ImplementationFactory != null)
                    {
                        throw new Exception();
                        //return ((ISolidProxied)proxied.ImplementationFactory(new DummyServiceProvider())).Service;
                    }
                    else
                    {
                        throw new Exception("!!!");
                    }
                }
                if (mustExist)
                {
                    throw new Exception($"Cannot find singleton service for {serviceType}.");
                }
                return DefaultValue(serviceType);
            }
        }

        private interface IServicesAdded { }
        private class ServicesAddedImplementation : IServicesAdded { }
        private class ServiceProviderForServiceCollection : IServiceProvider
        {
            public ServiceProviderForServiceCollection(IServiceCollection serviceCollection)
            {
                ServiceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            }

            public IServiceCollection ServiceCollection { get; }

            public object GetService(Type serviceType)
            {
                return ServiceCollection.GetSolidRpcService(serviceType, false);
            }
        }
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

        private static ICollection<Assembly> s_LoadedAssemblies;

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
                .Concat(typeof(ISolidRpcHost).GetMethods())
                .Concat(typeof(ISolidRpcOAuth2).GetMethods())
                .Concat(typeof(ISolidRpcOidc).GetMethods())
                .Concat(typeof(ICodeNamespaceGenerator).GetMethods())
                .Concat(typeof(ITypescriptGenerator).GetMethods())
                .Concat(typeof(INpmGenerator).GetMethods());

            var openApiParser = services.GetSolidRpcOpenApiParser();
            var solidRpcHostSpec = openApiParser.CreateSpecification(methods.ToArray()).WriteAsJsonString();
            methods.ToList().ForEach(m =>
            {
                services.AddSolidRpcBinding(m, (c) => {
                    c.OpenApiSpec = solidRpcHostSpec;


                    //
                    // disable security for content handler & token callback
                    //
                    var method = c.Methods.Single();
                    if (method.DeclaringType == typeof(ISolidRpcContentHandler))
                    {
                        c.DisableSecurity();
                    }
                    if (method.DeclaringType == typeof(ISolidRpcOAuth2))
                    {
                        c.DisableSecurity();
                    }
                    if (method.DeclaringType == typeof(ISolidRpcOidc))
                    {
                        c.DisableSecurity();
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
        public static TService GetSolidRpcService<TService>(this IServiceCollection services, bool mustExist = true) where TService : class
        {
            return (TService) services.GetSolidRpcService(typeof(TService), mustExist);
        }

        /// <summary>
        /// Returns the service provider for specified service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public static object GetSolidRpcService(this IServiceCollection services, Type serviceType, bool mustExist = true)
        {
            return DummyServiceProvider.GetService(services, serviceType, mustExist);
        }

        private static object DefaultValue(Type serviceType)
        {
            if (serviceType.IsValueType) return Activator.CreateInstance(serviceType);
            return null;
        }

        /// <summary>
        /// Registers a singleton service provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="implType"></param>
        /// <returns></returns>
        private static IServiceCollection RegisterSingletonService(this IServiceCollection services, Type serviceType, Type implType)
        {
            if (!services.Any(o => serviceType == o.ServiceType))
            {
                if(implType.GetConstructor(Type.EmptyTypes) == null)
                {
                    services.AddSingleton(implType, implType);
                    services.AddSingleton(serviceType, implType);
                }
                else
                {
                    var impl = Activator.CreateInstance(implType);
                    services.AddSingleton(implType, impl);
                    services.AddSingleton(serviceType, impl);
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
            //
            // check if already initialized
            //
            if(services.Any(o => o.ServiceType == typeof(IServicesAdded)))
            {
                return services;
            }
            services.AddSingleton<IServicesAdded, ServicesAddedImplementation>();

            //
            // initialize services
            //
            var assemblies = InitAssemblies();

            var attrs = assemblies.SelectMany(o => o.GetCustomAttributes(true))
                 .OfType<SolidRpcServiceAttribute>()
                 .GroupBy(o => o.ServiceType);
            foreach(var serviceAttrs in attrs)
            {
                var instances = serviceAttrs.Select(o => o.ServiceInstances).Distinct().Single();
                var sorted = serviceAttrs.OrderByDescending(o => GetDepth(o.ImplementationType)).ToList();
                if(instances == SolidRpcServiceInstances.One)
                {
                    sorted = sorted.Take(1).ToList();
                }
                foreach(var attr in sorted)
                {
                    switch (attr.ServiceLifetime)
                    {
                        case SolidRpcServiceLifetime.Singleton:
                            if(instances == SolidRpcServiceInstances.One)
                            {
                                services.RegisterSingletonService(attr.ServiceType, attr.ImplementationType);
                            }
                            else
                            {
                                services.AddSingleton(attr.ServiceType, attr.ImplementationType);
                            }
                            break;
                        case SolidRpcServiceLifetime.Scoped:
                            services.AddScoped(attr.ServiceType, attr.ImplementationType);
                            break;
                        case SolidRpcServiceLifetime.Transient:
                            services.AddTransient(attr.ServiceType, attr.ImplementationType);
                            break;
                        default:
                            throw new Exception("Cannot handle service lifetime type");
                    }
                }
            }
            
            services.GetSolidConfigurationBuilder();

            return services;
        }

        private static object GetDepth(Type t)
        {
            int depth = 0;
            while(t != null && t.BaseType != typeof(object))
            {
                depth++;
                t = t.BaseType;
            }
            return depth;
        }

        private static ICollection<Assembly> InitAssemblies()
        {
            if (s_LoadedAssemblies == null)
            {
                var assemblies = new List<Assembly>();
                LoadAssembly(assemblies, "SolidRpc.Abstractions");
                LoadAssembly(assemblies, "SolidRpc.OpenApi.Model");
                LoadAssembly(assemblies, "SolidRpc.OpenApi.Binder");
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AspNetCore", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AzFunctions", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AzFunctionsV1Extension", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AzFunctionsV2Extension", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AzQueue", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.AzSvcBus", false);
                LoadAssembly(assemblies, "SolidRpc.OpenApi.OAuth2", false);
                LoadAssembly(assemblies, "SolidRpc.Node", false);
                LoadAssembly(assemblies, "SolidRpc.Node.Arch", false);
                s_LoadedAssemblies = assemblies;
            }
            return s_LoadedAssemblies;
        }

        private static void LoadAssembly(ICollection<Assembly> assemblies, string assemblyName, bool required = true)
        {
            try
            {
                assemblies.Add(Assembly.Load(assemblyName));
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
            return services.GetSolidRpcService<ISolidRpcContentStore>();
        }

        /// <summary>
        /// Returns the static content provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IOpenApiParser GetSolidRpcOpenApiParser(this IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            return services.GetSolidRpcService<IOpenApiParser>();
        }


        /// <summary>
        /// Configures all the interfaces in supplied assembly
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementationAssembly"></param>
        /// <param name="configurator"></param>
        public static IServiceCollection AddSolidRpcBindings(
            this IServiceCollection sc, 
            Assembly interfaceAssembly, 
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
                Type impl = implementationAssembly.GetTypes()
                    .Where(o => !o.IsInterface)
                    .Where(o => t.IsAssignableFrom(o))
                    .OrderBy(o => o == t ? 1 : 0)
                    .SingleOrDefault();
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
        public static IEnumerable<ISolidRpcOpenApiConfig> AddSolidRpcSingletonBindings<T>(
            this IServiceCollection sc, 
            T impl, 
            Func<ISolidRpcOpenApiConfig, bool> configurator = null) where T:class 
        {
            if(impl != null)
            {
                sc.AddSingleton(impl);
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
            // configure method
            //
            var mc = sc.GetSolidConfigurationBuilder()
                .ConfigureInterfaceAssembly(mi.DeclaringType.Assembly)
                .ConfigureInterface(mi.DeclaringType)
                .ConfigureMethod(mi);

            var openApiProxyConfig = mc.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            sc.SetSolidRpcSecurityKeyFromConfig(openApiProxyConfig);
            sc.SetSolidRpcBaseUrlFromConfig(openApiProxyConfig);
            var enabled = configurator?.Invoke(openApiProxyConfig) ?? true;
            if(openApiProxyConfig.Enabled != mc.Enabled)
            {
                openApiProxyConfig.Enabled = enabled;
            }

            return openApiProxyConfig;
        }

        /// <summary>
        /// Sets the security key in supplied proxy config by getting the values from the configuration
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="openApiProxyConfig"></param>
        public static void SetSolidRpcSecurityKeyFromConfig(this IServiceCollection sc, ISolidRpcOpenApiConfig openApiProxyConfig)
        {
            var configuration = sc.GetSolidRpcService<IConfiguration>(false);
            if (configuration == null) return;
            var method = openApiProxyConfig.Methods.Single();
            var sKey = "SecurityKey";
            var keys = CreateConfigKeys(method, sKey);
            foreach (var key in keys)
            {
                var val = configuration[key];
                if (val != null)
                {
                    openApiProxyConfig.SetSecurityKey(sKey, val);
                    return;
                }
            }
        }

        /// <summary>
        /// Sets the base url in supplied proxy config by getting the values from the configuration
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="openApiProxyConfig"></param>
        public static void SetSolidRpcBaseUrlFromConfig(this IServiceCollection sc, ISolidRpcOpenApiConfig openApiProxyConfig)
        {
            var configuration = sc.GetSolidRpcService<IConfiguration>(false);
            if (configuration == null) return;
            var method = openApiProxyConfig.Methods.Single();
            var sKey = "BaseUrl";
            var keys = CreateConfigKeys(method, sKey);
            foreach (var key in keys)
            {
                var val = configuration[key];
                if (val != null)
                {
                    if (!val.EndsWith("/")) val = $"{val}/";
                    var baseUrl = new Uri(val);
                    openApiProxyConfig.ConfigureTransport<IHttpTransport>()
                        .SetMethodAddressTransformer((sp, url, mi) => {
                        if(mi == null) return new Uri(baseUrl, url.AbsolutePath.Substring(1));
                        return url;
                    });
                    return;
                }
            }
        }

        /// <summary>
        /// Adds the base url of the service to the configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sc"></param>
        /// <param name="baseUri"></param>
        public static void SetSolidRpcBaseUrlInConfig<T>(this IServiceCollection sc, Uri baseUri)
        {
            var cb = sc.GetConfigurationBuilder();
            cb.Add(new PropertiesConfigurationSource() { { $"{typeof(T).FullName.Replace(".", ":")}:BaseUrl", baseUri.ToString() } });
            sc.BuildConfiguration();
        }


        private static IEnumerable<string> CreateConfigKeys(MethodInfo method, string sKey)
        {
            var assemblyName = method.DeclaringType.Assembly.GetName().Name;
            var interfaceName = method.DeclaringType.FullName;
            if (interfaceName.StartsWith($"{assemblyName}."))
            {
                interfaceName = interfaceName.Substring(assemblyName.Length + 1);
            }
            var keys = $"SolidRpc.{assemblyName}.{interfaceName}.{method.Name}".Split('.').Distinct().ToArray();
            for(int i = keys.Length; i > 0; i--)
            {
                yield return string.Join(":", keys.Take(i).Union(new[] { sKey }));
            }
        }

        /// <summary>
        /// Returns the configuration builder
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ctor"></param>
        /// <param name="createSource"></param>
        /// <returns></returns>
        public static IConfigurationBuilder GetConfigurationBuilder(this IServiceCollection services, Func<IConfigurationBuilder> ctor = null, Func<IConfiguration, IConfigurationSource> createSource = null)
        {
            if (ctor == null) ctor = CreateConfigurationBuilder;
            if (createSource == null) createSource = CreateConfigurationSource;
            //
            // find configuration builder
            //
            var cbService = services.FirstOrDefault(o => o.ServiceType == typeof(IConfigurationBuilder));
            if (cbService == null)
            {
                services.Add(cbService = new ServiceDescriptor(typeof(IConfigurationBuilder), ctor()));
            }
            var cb = (IConfigurationBuilder)cbService.ImplementationInstance ?? throw new Exception("No implementation instance");

            //
            // grab existing configurations and add em to this one...
            //
            var configurationServices = services
                .Where(o => typeof(IConfiguration).IsAssignableFrom(o.ServiceType))
                .Where(o => o.ImplementationInstance != null)
                .ToList();
            foreach(var service in configurationServices)
            {
                var conf = (IConfiguration)service.ImplementationInstance;
                cb.Add(createSource(conf));
                services.Remove(service);
            }

            return cb;
        }

        private static IConfigurationSource CreateConfigurationSource(IConfiguration arg)
        {
            throw new NotImplementedException();
        }

        private static IConfigurationBuilder CreateConfigurationBuilder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the configuration builder
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static IConfiguration BuildConfiguration(this IServiceCollection services, Func<IConfigurationBuilder> ctor = null)
        {
            if (ctor == null) ctor = CreateConfigurationBuilder;
            var configuration = (IConfiguration)services
                .Where(o => typeof(IConfiguration).IsAssignableFrom(o.ServiceType))
                .Select(o => o.ImplementationInstance).FirstOrDefault();
            if(configuration != null)
            {
                return configuration;
            }

            var cb = services.GetConfigurationBuilder(ctor, null);
            services.Where(o => o.ServiceType == typeof(IConfigurationBuilder))
                .ToList().ForEach(o => services.Remove(o));

            configuration = cb.Build();
            services.AddSingleton(configuration);
            return configuration;
        }

        /// <summary>
        /// Returns the configuration builder
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcRateLimit(
            this IServiceCollection services,
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            var methods = typeof(ISolidRpcRateLimit).GetMethods();
            var openApiParser = services.GetSolidRpcOpenApiParser();
            var openApiSpec = openApiParser.CreateSpecification(methods.ToArray()).WriteAsJsonString();

            services.AddSolidRpcBindings(typeof(ISolidRpcRateLimit), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;

                // disable the implementation - if any..
                conf.GetAdviceConfig<ISolidProxyInvocationImplAdviceConfig>().Enabled = false;

                return configurator?.Invoke(conf) ?? true;
            });

            return services;
        }
     }
}
