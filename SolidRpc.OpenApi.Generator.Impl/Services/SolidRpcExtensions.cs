using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public interface IProxyConfig
        {
            /// <summary>
            /// The proxy type
            /// </summary>
            Type ProxyType { get; }

            /// <summary>
            /// The service lifetime
            /// </summary>
            ServiceLifetime Lifetime { get; }


            /// <summary>
            /// The implementation
            /// </summary>
            Type Implementation { get; }

            /// <summary>
            /// Constructs a new config with new lifetime
            /// </summary>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            IProxyConfig SetLifetime(ServiceLifetime lifetime);

            /// <summary>
            /// Sets the factory method to use when constructing implementations
            /// </summary>
            /// <param name="implementation"></param>
            /// <returns></returns>
            IProxyConfig SetImplementation(Type implementation);

            /// <summary>
            /// Resolves implementations from supplied assembly
            /// </summary>
            /// <param name="sc"></param>
            /// <param name="assembly"></param>
            /// <returns></returns>
            IProxyConfig SetAssemblyFactory(Assembly assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IProxyConfig<T> : IProxyConfig
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public class ProxyConfig<T> : IProxyConfig<T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lifetime"></param>
            /// <param name="implementation"></param>
            public ProxyConfig(ServiceLifetime lifetime, Type implementation)
            {
                Lifetime = lifetime;
                Implementation = implementation;
            }

            /// <summary>
            /// The proxy type
            /// </summary>
            public Type ProxyType => typeof(T);

            /// <summary>
            /// The service lifetime
            /// </summary>
            public ServiceLifetime Lifetime { get; }

            /// <summary>
            /// The factory
            /// </summary>
            public Type Implementation { get; }

            /// <summary>
            /// The factory method to use in order to create implementations
            /// </summary>
            /// <param name="implementation"></param>
            /// <returns></returns>
            public IProxyConfig SetImplementation(Type implementation)
            {
                return new ProxyConfig<T>(Lifetime, implementation);
            }

            /// <summary>
            /// Resolves implementations from supplied assembly
            /// </summary>
            /// <param name="assembly"></param>
            /// <returns></returns>
            public IProxyConfig SetAssemblyFactory(Assembly assembly)
            {
                var implementations = assembly.GetTypes()
                    .Where(o => ProxyType.IsAssignableFrom(o))
                    .Where(o => o.DeclaringType != typeof(RAMspecsExtensions))
                    .ToList();
                if (implementations.Count() == 0)
                {
                    throw new Exception($"Cannot find implementation of type {ProxyType} in assembly {assembly.GetName().Name}");
                }
                if (implementations.Count() > 1)
                {
                    throw new Exception($"Found more than one implementation oftype {ProxyType} in assembly {assembly.GetName().Name}");
                }
                var implementation = implementations.Single();
                return SetImplementation(implementation);
            }

            /// <summary>
            /// The object lifetime in the IoC
            /// </summary>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            public IProxyConfig SetLifetime(ServiceLifetime lifetime)
            {
                return new ProxyConfig<T>(lifetime, Implementation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        public static IProxyConfig<T> Configure<T>(
            System.Func<IProxyConfig, IProxyConfig> configure)
        {
            var config = (IProxyConfig<T>)new ProxyConfig<T>(
                ServiceLifetime.Transient,
                null);
            if (configure != null)
            {
                config = (IProxyConfig<T>)configure(config);
            }
            return config;
        }

        /// <summary>
        /// Configures the specified proxy
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void SetupProxy<TInterface, TImpl>(
            this IServiceCollection sc,
            Func<IProxyConfig, IProxyConfig> configure)
        {
            // add the configuration
            var config = Configure<TInterface>(configure);
            sc.AddSingleton(config);
            sc.Add(new ServiceDescriptor(typeof(TInterface), typeof(TImpl), config.Lifetime));
            if (config.Implementation != null)
            {
                sc.Add(new ServiceDescriptor(config.Implementation, config.Implementation, config.Lifetime));
            }
        }

        /// <summary>
        /// Base class for a proxy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Proxy<T> where T : class
        {
            /// <summary>
            /// The service provider
            /// </summary>
            protected IServiceProvider _serviceProvider;

            /// <summary>
            /// The configuration
            /// </summary>
            protected IProxyConfig<T> _config;

            /// <summary>
            /// Constructs a new proxy
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="config"></param>
            public Proxy(IServiceProvider serviceProvider, IProxyConfig<T> config)
            {
                _serviceProvider = serviceProvider;
                _config = config;
            }

            /// <summary>
            /// Returns the implementation for the proxy
            /// </summary>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            protected T GetImplementation()
            {
                var implementation = _config.Implementation ?? throw new Exception($"No implementation registered for service {_config.ProxyType.FullName}");
                return (T)_serviceProvider.GetRequiredService(implementation);
            }
        }
    }
}