using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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
                services.AddSingleton<IAzFunctionHandler>(funcHandler);
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
        /// <returns></returns>
        public static IServiceCollection AddStartupFunction<TService, TImpl>(this IServiceCollection services, Expression<Action<TService>> invocation) where TService : class where TImpl : class,TService
        {
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

            services.AddTransient<TService, TImpl>();
            return services;
        }

        /// <summary>
        /// Adds an http function
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="services"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpFunction<TService, TImpl>(this IServiceCollection services, Expression<Action<TService>> invocation) where TService : class where TImpl : class, TService
        {
            var mi = GetMethodInfo(invocation);
            services.AddSolidRpcBinding(mi, CreateOpenApiConfig(mi));
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
            so.Paths = new PathsObject(so);
            so.Paths[$"/{mi.Name}"] = new PathItemObject(so.Paths);
            so.Paths[$"/{mi.Name}"].Get = new OperationObject(so.Paths[$"/{mi.Name}"]);
            so.Paths[$"/{mi.Name}"].Get.OperationId = mi.Name;
            return so.WriteAsJsonString();
        }
    }
}
