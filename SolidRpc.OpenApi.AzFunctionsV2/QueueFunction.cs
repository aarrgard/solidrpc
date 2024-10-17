using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Handles queue triggers
    /// </summary>
    public static class QueueFunction
    {
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task Run(string message, string id, ILogger log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return FuncExecutor.ExecuteFunction(serviceProvider, log, message, async () =>
            {
                if (log.IsEnabled(LogLevel.Trace))
                {
                    log.LogTrace($"Picked up message({id}) from queue:{message}");
                }

                //
                // the message might be base64 encoded. Microsoft tends to break contracts on a regular basis...
                // 
                if (!message.StartsWith("{"))
                {
                    message = Encoding.UTF8.GetString(Convert.FromBase64String(message));
                }

                //
                // deserialize the message
                //
                var serFact = serviceProvider.GetRequiredService<ISerializerFactory>();
                HttpRequest httpReq;
                serFact.DeserializeFromString(message, out httpReq);

                var solidReq = new SolidHttpRequest();
                await solidReq.CopyFromAsync(httpReq, p => p);

                // invoke the method
                var localHandler = serviceProvider.GetRequiredService<LocalHandler>();
                var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
                var res = await methodInvoker.InvokeAsync(serviceProvider, localHandler, solidReq, cancellationToken);

                if (res.StatusCode >= 200 && res.StatusCode < 300)
                {
                    // this is ok
                    return;
                }
                throw new Exception($"Response code is not ok - {res.StatusCode}");
            });
        }
    }
}
