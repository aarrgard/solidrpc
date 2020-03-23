using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IHttpInvoker<>), typeof(HttpInvoker<>), ServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Invoker that sends messages using the http channel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpInvoker<T> : Invoker<T>, IHttpInvoker<T> where T : class
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="methodBinderStore"></param>
        public HttpInvoker(
            ILogger<HttpInvoker<T>> logger, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider, 
            IHttpClientFactory httpClientFactory) 
            : base(logger, methodBinderStore, serviceProvider)
        {
            HttpClientFactory = httpClientFactory;
        }

        private IHttpClientFactory HttpClientFactory { get; }

        protected override async Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, MethodInfo mi, object[] args)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if(methodBinding == null)
            {
                throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
            }
            var httpClientName = methodBinding.MethodBinder.OpenApiSpec.Title;
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);
            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args);


            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Sending data to remote host {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");
            }

            var httpClientReq = new HttpRequestMessage();
            var headers = new Dictionary<string, IEnumerable<string>>();
            //await MethodHeadersTransformer(invocation.ServiceProvider, headers, invocation.SolidProxyInvocationConfiguration.MethodInfo);
            foreach (var additionalHeader in headers)
            {
                httpClientReq.Headers.Add(additionalHeader.Key, additionalHeader.Value);
            }
            //
            // Add security key header
            //
            var securityKey = methodBinding.SecurityKey;
            if (securityKey != null)
            {
                httpClientReq.Headers.Add(securityKey.Value.Key, securityKey.Value.Value);
            }
            httpReq.CopyTo(httpClientReq);

            var httpClientResponse = await httpClient.SendAsync(httpClientReq);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            var returnType = mi.ReturnType;
            if(returnType.IsTaskType(out Type taskType))
            {
                returnType = taskType;
            }
            return methodBinding.ExtractResponse(returnType, httpResp);
        }
    }
}
