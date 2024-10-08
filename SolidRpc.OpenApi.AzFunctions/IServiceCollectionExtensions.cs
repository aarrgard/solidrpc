using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;
using SolidRpc.OpenApi.AzFunctions.Services;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        private static object s_mutex = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IAzFunctionHandler GetAzFunctionHandler(this IServiceCollection services)
        {
            lock(s_mutex)
            {
                services.AddSolidRpcSingletonServices();
                var funcHandler = (IAzFunctionHandler)services.Where(o => o.ServiceType == typeof(IAzFunctionHandler)).Select(o => o.ImplementationInstance).SingleOrDefault();
                if(funcHandler == null)
                {
                    var assemblyLocation = new FileInfo(typeof(AzFunctionHandler).Assembly.Location);
                    if (!assemblyLocation.Exists)
                    {
                        throw new Exception("Cannot find location of assebly.");
                    }

                    var baseDir = GetFolderWithHostJsonFile(assemblyLocation.Directory);

                    var assemblyNamePrefix = typeof(StartupSolidRpcServices).Assembly.GetName().Name;
                    var triggerAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(o => o.GetName().Name != typeof(StartupSolidRpcServices).Assembly.GetName().Name)
                        .Where(o => o.GetName().Name.StartsWith(assemblyNamePrefix))
                        .Where(o => o.GetTypes().Any(t => t.Name.EndsWith("Function")))
                        .Select(o => o.GetName().Name)
                        .Distinct()
                        .ToList();

                    if (triggerAssemblies.Count() != 1)
                    {
                        throw new Exception($"Did not find one trigger assembly({triggerAssemblies.Count()}:{string.Join(",", triggerAssemblies)})");
                    }

                    var triggerAssembly = Assembly.Load(new AssemblyName(triggerAssemblies.First()));
 
                    funcHandler = new AzFunctionHandler(baseDir, triggerAssembly);
                    services.AddSingleton(funcHandler);
                }
                return funcHandler;
            }
        }

        private static DirectoryInfo GetFolderWithHostJsonFile(DirectoryInfo directory)
        {
            if(directory.GetFiles("host.json").Length == 0)
            {
                if(directory.Parent == null) 
                {
                    return new DirectoryInfo("d:\\home\\site\\wwwroot");

                } 
                else
                {
                    return GetFolderWithHostJsonFile(directory.Parent);
                }

            }
            return directory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="services"></param>
        /// <param name="invocation"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzFunctionStartup<TService, TImpl>(this IServiceCollection services, Expression<Func<TService, Task>> invocation, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TService : class where TImpl : class,TService
        {
            services.AddServiceIfMissing<TService, TImpl>(serviceLifetime);
            services.AddAzFunctionTimer<TService>(invocation, "0 0 0 1 1 0", true);
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="invocation"></param>
        /// <param name="schedule"></param>
        /// <param name="runOnStartup"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzFunctionTimer<TService>(this IServiceCollection services, Expression<Func<TService, Task>> invocation, string schedule, bool runOnStartup = false) where TService : class
        {
            var mi = GetMethodInfo(invocation);
            var functionName = FunctionDef.CreateFunctionName("Timer", $".{typeof(TService).FullName}.{mi.Name}_{mi.GetParameters().Length}");
            var funcHandler = services.GetAzFunctionHandler();
            var azFunc = funcHandler.GetFunctions().SingleOrDefault(o => o.Name == functionName);

            var action = invocation.Compile();
            services.GetSolidRpcService<ITimerStore>().AddTimerAction(functionName, (sp, c) => action.Invoke(sp.GetRequiredService<TService>()));

            var timerFunc = funcHandler.CreateFunction<IAzTimerFunction>(functionName);
            timerFunc.RunOnStartup = runOnStartup;
            timerFunc.Schedule = schedule;
            timerFunc.TimerId = functionName;
            //timerFunc.Save();

            return services;
        }

        /// <summary>
        /// Adds an http function
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzFunctionHttp<TService, TImpl>(this IServiceCollection services, Expression<Action<TService>> invocation, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TService : class where TImpl : class, TService
        {
            services.AddServiceIfMissing<TService, TImpl>(serviceLifetime);
            return services.AddAzFunctionHttpInternal(invocation);
        }

        /// <summary>
        /// Adds an http function
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="invocation"></param>
        /// <param name="implementationFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzFunctionHttp<TService>(this IServiceCollection services, Expression<Action<TService>> invocation, Func<TService> implementationFactory) where TService : class
        {
            services.AddServiceIfMissing<TService>(implementationFactory);
            return services.AddAzFunctionHttpInternal(invocation);
        }
        private static IServiceCollection AddAzFunctionHttpInternal<TService>(this IServiceCollection services, Expression<Action<TService>> invocation) where TService : class
        {
            var mi = GetMethodInfo(invocation);
            var openApiParser = services.GetSolidRpcOpenApiParser();
            var openApiSpec = openApiParser.CreateSpecification(mi);
            services.AddSolidRpcBinding(mi, (c) =>
            {
                c.OpenApiSpec = openApiSpec.WriteAsJsonString();
                return true;
            });

            var funcHandler = services.GetAzFunctionHandler();

            return services;
        }

        /// <summary>
        /// Adds the transient service if not registered
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceIfMissing<TService, TImpl>(this IServiceCollection services, ServiceLifetime serviceLifetime) where TService : class where TImpl : class, TService
        {
            var service = services.FirstOrDefault(o => o.ServiceType == typeof(TService));
            if (service == null)
            {
                service = new ServiceDescriptor(typeof(TService), typeof(TImpl), serviceLifetime);
                services.Add(service);
            }
            if (service.Lifetime != serviceLifetime)
            {
                throw new Exception($"Cannot change service lifetime from {service.Lifetime} to {serviceLifetime}");
            }
            return services;
        }

        /// <summary>
        /// Adds the transient service if not registered
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="implementationFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceIfMissing<TService>(this IServiceCollection services, Func<TService> implementationFactory) where TService : class 
        {
            var service = services.FirstOrDefault(o => o.ServiceType == typeof(TService));
            if (service == null)
            {
                var impl = implementationFactory();
                service = new ServiceDescriptor(typeof(TService), impl);
                services.Add(service);
            }
            if (service.Lifetime != ServiceLifetime.Singleton)
            {
                throw new Exception($"Cannot change service lifetime from {service.Lifetime} to {ServiceLifetime.Singleton}");
            }
            return services;
        }

        private static MethodInfo GetMethodInfo(LambdaExpression expr)
        {
            if(expr.Body is MethodCallExpression mce)
            {
                return mce.Method;
            }
            throw new Exception("expression should be a method call.");
        }
    }
}
