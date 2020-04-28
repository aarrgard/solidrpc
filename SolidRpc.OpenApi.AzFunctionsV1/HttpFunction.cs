using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctionsV1
{
    /// <summary>
    /// Handles timer triggers
    /// </summary>
    public static class HttpFunction
    {
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return AzFunction.DoRun(async () => 
            {
                // copy data from req to generic structure
                // skip api prefix.
                var funcHandler = serviceProvider.GetRequiredService<IAzFunctionHandler>();
                var solidReq = new SolidHttpRequest();
                await solidReq.CopyFromAsync(req, funcHandler.GetPrefixMappings());

                // invoke the method
                var httpHandler = serviceProvider.GetRequiredService<HttpHandler>();
                var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
                var res = await methodInvoker.InvokeAsync(serviceProvider, httpHandler, solidReq, cancellationToken);

                log.Info($"C# HTTP trigger function processed a request - {res.StatusCode}");

                // return the response.
                var httpResponse = new HttpResponseMessage();
                await res.CopyToAsync(httpResponse, req);
                return httpResponse;
            });
        }
    }
}
