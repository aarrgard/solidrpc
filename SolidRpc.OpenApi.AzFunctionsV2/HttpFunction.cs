using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
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
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ILogger log, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            //
            // for some reason the port is not added to the request
            //
            var addrTrans = serviceProvider.GetRequiredService<IMethodAddressTransformer>();
            var baseAddress = addrTrans.BaseAddress;
            if (baseAddress.Host == req.RequestUri.Host)
            {
                var ub = new UriBuilder(req.RequestUri);
                ub.Port = baseAddress.Port;
                req.RequestUri = ub.Uri;
            }
                
            // copy data from req to generic structure
            // skip api prefix.
            var solidReq = new SolidHttpRequest();
            await solidReq.CopyFromAsync(req, addrTrans.RewritePath);

            // invoke the method
            var httpHandler = serviceProvider.GetRequiredService<HttpHandler>();
            var methodInvoker = serviceProvider.GetRequiredService<IMethodInvoker>();
            var res = await methodInvoker.InvokeAsync(serviceProvider, httpHandler, solidReq, cancellationToken);

            //log.Info($"C# HTTP trigger function processed a request - {res.StatusCode}");

            // return the response.
            var httpResponse = new HttpResponseMessage();
            await res.CopyToAsync(httpResponse, req);
            return httpResponse;
        }
    }
}
