using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// The implementation of the method invoker.
    /// </summary>
    public class MethodInvoker : IMethodInvoker
    {
        public MethodInvoker(ILogger<MethodInvoker> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        public ILogger Logger { get; }
        public IServiceProvider ServiceProvider { get; }

        public async Task<IHttpResponse> InvokeAsync(IHttpRequest request, CancellationToken cancellation = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<IHttpResponse> InvokeAsync(IHttpRequest request, IMethodInfo methodInfo, CancellationToken cancellation = default(CancellationToken))
        {
            var args = await methodInfo.ExtractArgumentsAsync(request);

            // invoke
            var proxy = (ISolidProxy)ServiceProvider.GetService(methodInfo.MethodInfo.DeclaringType);
            if (proxy == null)
            {
                throw new Exception($"Failed to resolve proxy for type {methodInfo.MethodInfo.DeclaringType}");
            }

            // return response
            var resp = new SolidRpc.OpenApi.Binder.HttpResponse();
            try
            {
                var res = await proxy.InvokeAsync(methodInfo.MethodInfo, args);

                await methodInfo.BindResponseAsync(resp, res, methodInfo.MethodInfo.ReturnType);
            }
            catch (Exception ex)
            {
                // handle exception
                Logger.LogError(ex, "Service returned an excpetion - sending to client");
                await methodInfo.BindResponseAsync(resp, ex, methodInfo.MethodInfo.ReturnType);
            }
            return resp;

        }
    }
}
