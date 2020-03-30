using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctionsV2
{
    /// <summary>
    /// Handles timer triggers
    /// </summary>
    public static class QueueFunction
    {
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task Run(string message, ILogger log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            //
            // deserialize the message
            //
            var serFact = serviceProvider.GetRequiredService<ISerializerFactory>();
            HttpRequest httpReq;
            serFact.DeserializeFromString(message, out httpReq);

            var solidReq = new SolidHttpRequest();
            await solidReq.CopyFromAsync(httpReq);

            // invoke the method
            var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
            var res = await methodInvoker.InvokeAsync(solidReq, cancellationToken);

            if(res.StatusCode >= 200 && res.StatusCode < 300)
            {
                // this is ok
                return;
            }
            throw new Exception($"Response code is not ok - {res.StatusCode}");
        }
    }
}
