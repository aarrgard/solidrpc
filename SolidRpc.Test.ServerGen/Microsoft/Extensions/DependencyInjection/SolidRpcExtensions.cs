using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

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

            /// <summary>
            /// Adds an interceptor to the proxy
            /// </summary>
            /// <param name="interceptor"></param>
            /// <returns></returns>
            IProxyConfig AddInterceptor(ProxyInterceptor interceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IProxyConfig<T> : IProxyConfig
        {
            /// <summary>
            /// Intercepts a call
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="impl"></param>
            /// <param name="methodInfo"></param>
            /// <param name="args"></param>
            /// <param name="last"></param>
            /// <returns></returns>
            /// <typeparam name="T"></typeparam>
            Task<T> InterceptAsync<T>(
                IServiceProvider serviceProvider,
                object impl,
                MethodInfo methodInfo,
                object[] args,
                Func<Task<T>> last = null);

            /// <summary>
            /// Intercepts a call
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="impl"></param>
            /// <param name="methodInfo"></param>
            /// <param name="args"></param>
            /// <param name="last"></param>
            /// <returns></returns>
            Task InterceptAsync(
                IServiceProvider serviceProvider,
                object impl,
                MethodInfo methodInfo,
                object[] args,
                Func<Task> last = null);
        }

        /// <summary>
        /// The proxy interceptor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="next"></param>
        /// <param name="sp"></param>
        /// <param name="impl"></param>
        /// <param name="mi"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public delegate Task<object> ProxyInterceptor(
            Func<Task<object>> next,
            IServiceProvider sp,
            object impl,
            MethodInfo mi,
            object[] args);

        /// <summary>
        /// 
        /// </summary>
        public class ProxyConfig<T> : IProxyConfig<T> where T : class
        {
            /// <summary>
            /// Empty constructor
            /// </summary>
            public ProxyConfig() : this(ServiceLifetime.Transient, null, new ProxyInterceptor[0])
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="lifetime"></param>
            /// <param name="implementation"></param>
            /// <param name="interceptors"></param>
            public ProxyConfig(ServiceLifetime lifetime, Type implementation, ProxyInterceptor[] interceptors)
            {
                Lifetime = lifetime;
                Implementation = implementation;
                Interceptors = interceptors;
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
            /// The factory
            /// </summary>
            public ProxyInterceptor[] Interceptors { get; }

            /// <summary>
            /// The factory method to use in order to create implementations
            /// </summary>
            /// <param name="implementation"></param>
            /// <returns></returns>
            public IProxyConfig SetImplementation(Type implementation)
            {
                return new ProxyConfig<T>(Lifetime, implementation, Interceptors);
            }

            /// <summary>
            /// Resolves implementations from supplied assembly
            /// </summary>
            /// <param name="assembly"></param>
            /// <returns></returns>
            public IProxyConfig SetAssemblyFactory(Assembly assembly)
            {
                var implementations = assembly.GetTypes()
                    .Where(t => !typeof(Proxy<T>).IsAssignableFrom(t))
                    .Where(ProxyType.IsAssignableFrom)
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
            /// Adds an interceptor
            /// </summary>
            /// <param name="interceptor"></param>
            /// <returns></returns>
            public IProxyConfig AddInterceptor(ProxyInterceptor interceptor)
            {
                return new ProxyConfig<T>(Lifetime, Implementation, Interceptors.Union(new[] { interceptor }).ToArray());
            }

            /// <summary>
            /// The object lifetime in the IoC
            /// </summary>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            public IProxyConfig SetLifetime(ServiceLifetime lifetime)
            {
                return new ProxyConfig<T>(lifetime, Implementation, Interceptors);
            }

            /// <summary>
            /// Intercepts an invocation
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="impl"></param>
            /// <param name="methodInfo"></param>
            /// <param name="args"></param>
            /// <param name="last"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public async Task InterceptAsync(IServiceProvider serviceProvider, object impl, MethodInfo methodInfo, object[] args, Func<Task> last = null)
            {
                await InterceptInternalAsync<object>(0, serviceProvider, impl, methodInfo, args, async () => { await last(); return null; });
            }

            /// <summary>
            /// Intercepts an invocation
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="impl"></param>
            /// <param name="methodInfo"></param>
            /// <param name="args"></param>
            /// <param name="last"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public Task<TRes> InterceptAsync<TRes>(IServiceProvider serviceProvider, object impl, MethodInfo methodInfo, object[] args, Func<Task<TRes>> last = null)
            {
                return InterceptInternalAsync(0, serviceProvider, impl, methodInfo, args, last);
            }

            private Task<TRes> InterceptInternalAsync<TRes>(int idx, IServiceProvider serviceProvider, object impl, MethodInfo methodInfo, object[] args, Func<Task<TRes>> last)
            {
                if (idx >= Interceptors.Length)
                {
                    return last();
                }
                return InvokeInterceptorAsync(idx, serviceProvider, impl, methodInfo, args, last);
            }

            private async Task<TRes> InvokeInterceptorAsync<TRes>(int idx, IServiceProvider serviceProvider, object impl, MethodInfo methodInfo, object[] args, Func<Task<TRes>> last)
            {
                return (TRes)await Interceptors[idx](async () =>
                {
                    return await InterceptInternalAsync(idx + 1, serviceProvider, impl, methodInfo, args, last);
                },
                serviceProvider, impl, methodInfo, args);
            }
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
            Func<IProxyConfig, IProxyConfig> configure) where TInterface : class
        {
            var config = sc.ConfigureInterface<TInterface>(configure);
            sc.Add(new ServiceDescriptor(typeof(TInterface), typeof(TImpl), config.Lifetime));
            if (config.Implementation != null)
            {
                sc.Add(new ServiceDescriptor(config.Implementation, config.Implementation, config.Lifetime));
            }
            sc.SetupInvoker<TInterface>();
        }

        /// <summary>
        /// Base class for a proxy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Proxy<T> where T : class
        {
            /// <summary>
            /// Returns the method info
            /// </summary>
            /// <param name="name"></param>
            /// <param name="types"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            protected static MethodInfo GetMethodInfo(string name, Type[] types)
            {
                var method = typeof(T)
                    .GetMethods()
                    .Where(o => o.Name == name)
                    .Where(o => ParameterMatches(o.GetParameters(), types))
                    .FirstOrDefault();
                if (method == null)
                {
                    throw new Exception("Cannot find method");
                }
                return method;
            }
            private static bool ParameterMatches(ParameterInfo[] params1, Type[] params2)
            {
                if (params1.Length != params2.Length)
                {
                    return false;
                }
                for (int i = 0; i < params1.Length; i++)
                {
                    var t1 = FixType(params1[i].ParameterType);
                    var t2 = FixType(params2[i]);
                    if (t1 != t2)
                    {
                        return false;
                    }
                }
                return true;
            }
            private static Type FixType(Type type)
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return type.GetGenericArguments()[0];
                    }
                }
                return type;
            }

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

        /// <summary>
        /// Configures the specified proxy
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="sc"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void SetupInvoker<TInterface>(
            this IServiceCollection sc) where TInterface : class
        {
            sc.AddTransient<IInvoker<TInterface>, Invoker<TInterface>>();
        }

        /// <summary>
        /// The invoker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Invoker<T> : IInvoker<T> where T : class
        {
            private static ConcurrentDictionary<MethodInfo, MethodInfo> _invocs = new ConcurrentDictionary<MethodInfo, MethodInfo>();

            public Invoker(T service)
            {
                Service = service;
            }

            private T Service { get; }

            public IMethodBinding GetMethodBinding(Expression<Action<T>> action)
            {
                throw new NotImplementedException();
            }

            public IMethodBinding GetMethodBinding<TResult>(Expression<Func<T, TResult>> func)
            {
                throw new NotImplementedException();
            }

            public IMethodBinding GetMethodBinding(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }

            public Task<Uri> GetUriAsync(Expression<Action<T>> action, bool includeQueryString = true)
            {
                throw new NotImplementedException();
            }

            public Task<Uri> GetUriAsync<TRes>(Expression<Func<T, TRes>> func, bool includeQueryString = true)
            {
                throw new NotImplementedException();
            }

            public async Task InvokeAsync(Expression<Action<T>> action, Func<InvocationOptions, InvocationOptions> invocationOptions = null)
            {
                using (SetupInvocationOptions(invocationOptions))
                {
                    var (mi, args) = GetMethodInfo(action);
                    var res = mi.Invoke(Service, args);
                    var task = res as Task;
                    if (task != null)
                    {
                        await task;
                    }
                }
            }

            public TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func, Func<InvocationOptions, InvocationOptions> invocationOptions = null)
            {
                using (SetupInvocationOptions(invocationOptions))
                {
                    var (mi, args) = GetMethodInfo(func);
                    return (TResult)mi.Invoke(Service, args);
                }
            }

            public Task<object> InvokeAsync(MethodInfo methodInfo, IEnumerable<object> args, Func<InvocationOptions, InvocationOptions> invocationOptions = null)
            {
                return (Task<object>)_invocs.GetOrAdd(methodInfo, _ =>
                {
                    var retVal = _.ReturnType;
                    if (retVal.IsGenericType)
                    {
                        if (retVal.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            retVal = retVal.GetGenericArguments()[0];
                        }
                    }
                    return GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(o => o.Name == nameof(InvokeAsync))
                        .Where(o => o.IsGenericMethod)
                        .Single()
                        .MakeGenericMethod(retVal);
                }).Invoke(this, new object[] { methodInfo, args, invocationOptions });
            }
            private async Task<object> InvokeAsync<TRes>(MethodInfo methodInfo, object[] args, Func<InvocationOptions, InvocationOptions> invocationOptions = null)
            {
                using (SetupInvocationOptions(invocationOptions))
                {
                    var res = methodInfo.Invoke(Service, args);
                    var task = res as Task<TRes>;
                    if (task != null)
                    {
                        return await task;
                    }
                    else
                    {
                        return (TRes)res;
                    }
                }
            }

            private IDisposable SetupInvocationOptions(Func<InvocationOptions, InvocationOptions> invocationOptions)
            {
                var opts = InvocationOptions.Current;
                opts = invocationOptions?.Invoke(opts) ?? opts;
                return opts.Attach();
            }

            protected static (MethodInfo, object[]) GetMethodInfo(LambdaExpression expr)
            {
                if (expr.Body is MethodCallExpression mce)
                {
                    var args = new List<object>();
                    foreach (var argument in mce.Arguments)
                    {
                        var le = Expression.Lambda(argument);
                        args.Add(le.Compile().DynamicInvoke());
                    }

                    return (mce.Method, args.ToArray());
                }
                if (expr.Body is MemberExpression me)
                {
                    if (me.Member is PropertyInfo pi)
                    {
                        return (pi.GetMethod, null);
                    }
                    throw new Exception();
                }
                throw new Exception("expression should be a method call.");
            }
        }

        /// <summary>
        /// Some helper methods
        /// </summary>
        public static IProxyConfig<TInterface> ConfigureInterface<TInterface>(this IServiceCollection sc, Func<IProxyConfig, IProxyConfig> configure) where TInterface : class
        {
            var existingConfigService = sc.FirstOrDefault(o => o.ServiceType == typeof(IProxyConfig<TInterface>));
            var config = existingConfigService?.ImplementationInstance as IProxyConfig<TInterface> ?? new ProxyConfig<TInterface>();
            if (configure != null)
            {
                config = (IProxyConfig<TInterface>)configure(config);
            }
            if (existingConfigService != null)
            {
                sc.Remove(existingConfigService);
            }
            sc.AddSingleton(config);
            return config;
        }
    }
}