using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctionsV2
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
        public static async Task<IActionResult> Run(HttpRequest req, ILogger log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var oldServiceProvider = req.HttpContext.RequestServices;
            try
            {
                req.HttpContext.RequestServices = serviceProvider;
                var methodInvoker = req.HttpContext.RequestServices.GetRequiredService<IMethodInvoker>();
                var solidReq = new SolidHttpRequest();
                await solidReq.CopyFromAsync(req);

                var res = await methodInvoker.InvokeAsync(solidReq, req.HttpContext.RequestAborted);

                log.LogInformation($"C# HTTP trigger function processed a request - {res.StatusCode}");

                return await res.CreateActionResult();
            }
            finally
            {
                req.HttpContext.RequestServices = oldServiceProvider;
            }
        }
    }
}
