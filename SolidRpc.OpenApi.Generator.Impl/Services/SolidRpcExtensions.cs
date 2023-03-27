using System;

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
            System.Type ProxyType { get; }

            /// <summary>
            /// The service lifetime
            /// </summary>
            ServiceLifetime Lifetime { get; }


            /// <summary>
            /// The implementation factory
            /// </summary>
            Func<IServiceProvider, object> Factory { get; }

            /// <summary>
            /// Constructs a new config with new lifetime
            /// </summary>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            IProxyConfig SetLifetime(ServiceLifetime lifetime);

            /// <summary>
            /// Sets the factory method to use when constructing implementations
            /// </summary>
            /// <param name="factory"></param>
            /// <returns></returns>
            IProxyConfig SetFactory(Func<IServiceProvider, object> factory);
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
            public ProxyConfig(ServiceLifetime lifetime, Func<IServiceProvider, object> factory)
            {
                Lifetime = lifetime;
                Factory = factory;
            }

            /// <summary>
            /// The proxy type
            /// </summary>
            public Type ProxyType => typeof(T);

            /// <summary>
            /// The service lifetime
            /// </summary>
            public ServiceLifetime Lifetime { get; }

            public Func<IServiceProvider, object> Factory { get; }

            public IProxyConfig SetFactory(Func<IServiceProvider, object> factory)
            {
                return new ProxyConfig<T>(Lifetime, factory);
            }

            public IProxyConfig SetLifetime(ServiceLifetime lifetime)
            {
                return new ProxyConfig<T>(lifetime, Factory);
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
                sp => { throw new Exception($"Cannot create implementation for {typeof(T)}"); });
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
                return (T)_config.Factory(_serviceProvider);
            }
        }
    }
}