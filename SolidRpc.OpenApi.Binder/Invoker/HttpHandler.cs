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
        public HttpHandler(ILogger<HttpHandler> logger, IServiceProvider serviceProvider)
            :base(logger, serviceProvider)
        {
        }

        public IHttpClientFactory HttpClientFactory => ServiceProvider.GetRequiredService<IHttpClientFactory>();

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

        public override async Task<IHttpResponse> InvokeAsync(IMethodBinding methodBinding, IHttpTransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            var httpClientName = methodBinding.MethodBinder.OpenApiSpec.Title;
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);

            var httpClientReq = new HttpRequestMessage();
            httpReq.CopyTo(httpClientReq);

            var httpClientResponse = await httpClient.SendAsync(httpClientReq);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            return httpResp;
        }
    }
}
