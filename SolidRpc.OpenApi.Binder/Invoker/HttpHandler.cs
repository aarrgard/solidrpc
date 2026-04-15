using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(HttpHandler), typeof(HttpHandler))]
[assembly: SolidRpcService(typeof(ITransportHandler), typeof(HttpHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Represents the HttpHandler
    /// </summary>
    public class HttpHandler : TransportHandler<IHttpTransport>
    {
        public static readonly IDictionary<int, Func<TimeSpan>> DefaultRetryPolicy = new Dictionary<int, Func<TimeSpan>>()
        {
            { 503, () => TimeSpan.FromSeconds(60) }
        };

        public HttpHandler(
            ILogger<HttpHandler> logger,
            IMethodBinderStore methodBinderStore)
            :base(logger, methodBinderStore)
        {
        }

        public override void Configure(IMethodBinding methodBinding, IHttpTransport transport)
        {
            transport.Path = methodBinding.RelativePath;

            if (methodBinding.IsLocal)
            {
                transport.BaseAddress = methodBinding.MethodBinder.HostedAddress;
            }
            else
            {
                transport.BaseAddress = TransformAddress(methodBinding, transport, methodBinding.MethodBinder.OpenApiSpec.BaseAddress, null);
            }
            var operationAddress = new Uri(transport.BaseAddress, transport.Path);
            operationAddress = TransformAddress(methodBinding, transport, operationAddress, methodBinding.MethodInfo);
            if (transport.OperationAddress != null && transport.OperationAddress != operationAddress)
            {
                throw new Exception($"Operation address({operationAddress}) has already been configured.");
            }
            transport.OperationAddress = operationAddress;
        }

        private Uri TransformAddress(IMethodBinding methodBinding, IHttpTransport transport, Uri address, MethodInfo methodInfo)
        {
            var serviceProvider = methodBinding.MethodBinder.ServiceProvider;
            if (transport.MethodAddressTransformer != null)
            {
                address = transport.MethodAddressTransformer(serviceProvider, address, methodInfo);
            }
            else if (methodBinding.IsLocal)
            {
                var methodAddressResolver = (IMethodAddressTransformer)serviceProvider.GetService(typeof(IMethodAddressTransformer));
                if (methodAddressResolver != null)
                {
                    address = methodAddressResolver.TransformUri(address, methodInfo);
                }
            }
            return address;
        }

        public override async Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, IHttpTransport transport, IHttpRequest httpReq, CancellationToken cancellationToken)
        {
            InvocationOptions.GetOptions(methodBinding.MethodInfo).TryGetValue(nameof(IHttpClientFactory)+".HttpClientName", out string httpClientName);
            if(string.IsNullOrEmpty(httpClientName))
            {
                httpClientName = methodBinding.MethodBinder.OpenApiSpec.Title;
            }
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(httpClientName);

            var timeouts = new Dictionary<int, DateTimeOffset>();
            foreach(var x in transport.RetryPolicy ?? DefaultRetryPolicy)
            {
                timeouts[x.Key] = DateTimeOffset.Now.Add(x.Value());
            }

            var httpClientResponse = await SendWithRetryAsync(httpClient, httpReq, timeouts);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            return httpResp;
        }

        private async Task<HttpResponseMessage> SendWithRetryAsync(HttpClient httpClient, IHttpRequest httpReq, IDictionary<int, DateTimeOffset> retryPolicy)
        {
            int count = 0;
            while(true)
            {

                var httpClientReq = new HttpRequestMessage();
                httpReq.CopyTo(httpClientReq);

                var resp = await httpClient.SendAsync(httpClientReq);
                if(retryPolicy.TryGetValue((int)resp.StatusCode, out DateTimeOffset retryTimeout))
                {
                    if(retryTimeout > DateTimeOffset.Now)
                    {
                        var wait = (int)(Math.Pow(2, count)) * 1000;
                        var waitMills = Math.Max((retryTimeout - DateTimeOffset.Now).TotalMilliseconds, 0);
                        waitMills = Math.Min(waitMills, wait);
                        await Task.Delay((int)waitMills);
                        count++;
                        continue;
                    }
                }

                return resp;
            }
        }
    }
}
