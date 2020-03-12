using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Handles timer triggers
    /// </summary>
    public static class TimerFunction
    {
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceType"></param>
        /// <param name="methodName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task Run(TimerInfo myTimer, ILogger log, IServiceProvider serviceProvider, Type serviceType, string methodName, CancellationToken cancellationToken)
        {
            if (serviceType == null)
            {
                throw new ArgumentException("No service type supplied");
            }
            var methodInfo = serviceType.GetMethod(methodName);
            if(methodInfo == null)
            {
                throw new ArgumentException($"Service({serviceType.FullName}) does not define the method {methodName}");
            }
 
            var parameters = methodInfo.GetParameters();
            var args = new object[parameters.Length];
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = ResolveArgument(serviceProvider, parameters[i].ParameterType, cancellationToken);
            }

            var methodInvoker = (IMethodInvoker)serviceProvider.GetService(typeof(IMethodInvoker));
            var res = await methodInvoker.InvokeInternalAsync(methodInfo, args, cancellationToken);
            var resTask = res as Task;
            if (resTask != null)
            {
                await resTask;
            }
        }

        private static object ResolveArgument(IServiceProvider serviceProvider, Type parameterType, CancellationToken cancellationToken)
        {
            if(parameterType == typeof(CancellationToken))
            {
                return cancellationToken;
            }
            else
            {
                return serviceProvider.GetService(parameterType);
            }
        }
    }
}
