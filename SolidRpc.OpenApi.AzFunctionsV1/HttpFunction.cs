using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
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
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var solidReq = new SolidHttpRequest();
            await solidReq.CopyFromAsync(req, "/api");

            var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
            var res = await methodInvoker.InvokeAsync(solidReq, cancellationToken);

            log.Info($"C# HTTP trigger function processed a request - {res.StatusCode}");

            var httpResponse = new HttpResponseMessage();
            await res.CopyToAsync(httpResponse);
            return httpResponse;
        }
    }
}
