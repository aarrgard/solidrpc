using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using SolidRpc.Abstractions.OpenApi.Transport;
using System.Linq;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcOpenApiAdvice(
            ILogger<SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice>> logger,
            IServiceProvider serviceProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        private ILogger Logger { get; }
        private IServiceProvider ServiceProvider { get; }
        private IMethodBinderStore MethodBinderStore => ServiceProvider.GetRequiredService<IMethodBinderStore>();
        private IHttpClientFactory HttpClientFactory => ServiceProvider.GetRequiredService<IHttpClientFactory>();
        private MethodHeadersTransformer MethodHeadersTransformer { get; set; }
        private IMethodBinding MethodBinding { get; set; }
        private KeyValuePair<string, string>? SecurityKey { get; set; }
        private Uri OperationAddress { get; set; }

        /// <summary>
        /// Confugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            if(config.InvocationConfiguration.HasImplementation)
            {
                return false;
            }
            SecurityKey = config.SecurityKey;
            MethodHeadersTransformer = config.HttpTransport?.MethodHeadersTransformer ?? ((o1, o2, o3) => Task.CompletedTask);
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                config.OpenApiSpec,
                config.InvocationConfiguration.MethodInfo,
                config.GetTransports(),
                config.SecurityKey
            ).First();
            OperationAddress = MethodBinding.Transports.OfType<IHttpTransport>().Select(o => o.OperationAddress).FirstOrDefault();
            return true;
        }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var httpClientName = MethodBinding.MethodBinder.OpenApiSpec.Title;
            if(Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);
            var httpReq = new SolidHttpRequest();
            await MethodBinding.BindArgumentsAsync(httpReq, invocation.Arguments, OperationAddress);

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Sending data to remote host {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");
            }

            var httpClientReq = new HttpRequestMessage();
            var headers = new Dictionary<string, IEnumerable<string>>();
            await MethodHeadersTransformer(invocation.ServiceProvider, headers, invocation.SolidProxyInvocationConfiguration.MethodInfo);
            foreach (var additionalHeader in headers)
            {
                httpClientReq.Headers.Add(additionalHeader.Key, additionalHeader.Value);
            }
            if(SecurityKey != null)
            {
                httpClientReq.Headers.Add(SecurityKey.Value.Key, SecurityKey.Value.Value);
            }
            httpReq.CopyTo(httpClientReq);

            var httpClientResponse = await httpClient.SendAsync(httpClientReq);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            return MethodBinding.ExtractResponse<TAdvice>(httpResp);
        }
    }
}
