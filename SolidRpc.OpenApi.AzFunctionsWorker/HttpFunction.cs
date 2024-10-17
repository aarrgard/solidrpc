using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
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
        public class ResponseResult : IActionResult
        {
            private IHttpResponse res;

            public ResponseResult(IHttpResponse res)
            {
                this.res = res;
            }

            public async Task ExecuteResultAsync(ActionContext context)
            {
                await res.CopyToAsync(context.HttpContext.Response);
            }
        }

        public class ErrorResult : IActionResult
        {
            public Task ExecuteResultAsync(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = 500;
                return Task.CompletedTask;
            }
        }
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<IActionResult> Run(HttpRequest req, ILogger log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return FuncExecutor.ExecuteFunction<IActionResult>(serviceProvider, log, async () => {
                // copy data from req to generic structure
                // skip api prefix.
                var solidReq = new SolidHttpRequest();
                await solidReq.CopyFromAsync(req);

                // invoke the method
                var httpHandler = serviceProvider.GetRequiredService<HttpHandler>();
                var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
                var res = await methodInvoker.InvokeAsync(serviceProvider, httpHandler, solidReq, cancellationToken);

                return new ResponseResult(res);
            }, () => Task.FromResult<IActionResult>(new ErrorResult()));
        }
    }
}
