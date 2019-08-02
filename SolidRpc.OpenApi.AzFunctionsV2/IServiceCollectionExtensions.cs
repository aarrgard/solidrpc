using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IAzFunctionHandler GetAzFunctionHandler(this IServiceCollection services)
        {
            var funcHandler = (IAzFunctionHandler)services.Where(o => o.ServiceType == typeof(AzFunctionHandler)).Select(o => o.ImplementationInstance).SingleOrDefault();
            if(funcHandler == null)
            {
                var assemblyLocattion = new FileInfo(typeof(AzFunctionHandler).Assembly.Location);
                if (!assemblyLocattion.Exists)
                {
                    throw new Exception("Cannot find location of assebly.");
                }
                if (assemblyLocattion.Directory.Name != "bin")
                {
                    throw new Exception("Assemblies are not placed in the bin folder.");
                }
                funcHandler = new AzFunctionHandler(assemblyLocattion.Directory.Parent);
                services.AddSingleton(funcHandler);
            }
            return funcHandler;
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
        public static IServiceCollection AddAzFunctionStartup<TService, TImpl>(this IServiceCollection services, Expression<Action<TService>> invocation, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TService : class where TImpl : class,TService
        {
            services.AddServiceIfMissing<TService, TImpl>(serviceLifetime);

            var mi = GetMethodInfo(invocation);
            var functionName = $"Timer_{typeof(TService).FullName}.{mi.Name}".Replace(".", "_");
            var funcHandler = services.GetAzFunctionHandler();
            var initFunc = funcHandler.Functions.SingleOrDefault(o => o.Name == functionName);
            if (initFunc != null && false == initFunc is IAzTimerFunction)
            {
                initFunc.Delete();
                initFunc = null;
            }
            var timerFunc = (IAzTimerFunction)initFunc;
            if (timerFunc == null)
            {
                timerFunc = funcHandler.CreateTimerFunction(functionName);
            }
            timerFunc.RunOnStartup = true;
            timerFunc.ServiceType = typeof(TService).FullName;
            timerFunc.MethodName = mi.Name;
            timerFunc.Save();

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

            var mi = GetMethodInfo(invocation);
            services.AddSolidRpcBinding(mi, CreateOpenApiConfig(mi));
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
            if(service.Lifetime != serviceLifetime)
            {
                throw new Exception($"Cannot change service lifetime from {service.Lifetime} to {serviceLifetime}");
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

        private static string CreateOpenApiConfig(MethodInfo mi)
        {
            var so = new SwaggerObject(null)
            {
                BasePath = $"/{mi.DeclaringType.FullName.Replace('.', '/')}",
            };

            var op = so.GetGetOperation($"/{mi.Name}");
            op.OperationId = mi.Name;

            foreach(var p in mi.GetParameters())
            {
                var param = op.GetParameter(p.Name);
                param.SetTypeInfo(p.ParameterType);
            }
            op.GetResponse("200").SetTypeInfo(mi.ReturnType);

            return so.WriteAsJsonString();
        }
    }
}
