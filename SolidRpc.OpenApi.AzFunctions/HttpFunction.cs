using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using System;
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
        /// <param name="methodInvoker"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IActionResult> Run(HttpRequest req, ILogger log, IMethodInvoker methodInvoker, CancellationToken cancellationToken)
        {
            var solidReq = new SolidHttpRequest();
            await solidReq.CopyFromAsync(req);

            var res = await methodInvoker.InvokeAsync(solidReq, req.HttpContext.RequestAborted);

            log.LogInformation($"C# HTTP trigger function processed a request - {res.StatusCode}");

            return await res.CreateActionResult();
        }
    }
}
