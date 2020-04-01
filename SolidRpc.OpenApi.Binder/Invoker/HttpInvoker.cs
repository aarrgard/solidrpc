using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IHttpInvoker<>), typeof(HttpInvoker<>), SolidRpcServiceLifetime.Scoped)]
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

        public Task<Uri> GetUriAsync(Expression<Action<T>> action, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(action);
            return GetUriAsync(mi, args, includeQueryString);
        }

        public Task<Uri> GetUriAsync<TRes>(Expression<Func<T, TRes>> func, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(func);
            return GetUriAsync(mi, args, includeQueryString);
        }

        private async Task<Uri> GetUriAsync(MethodInfo mi, object[] args, bool includeQueryString)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if (methodBinding == null)
            {
                throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
            }
            var operationAddress = methodBinding.Transports
                .OfType<IHttpTransport>().Select(o => o.OperationAddress)
                .FirstOrDefault();
            var req = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(req, args, operationAddress);
            return req.CreateUri(includeQueryString);
        }

        protected override async Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, SolidRpcHostInstance targetInstance, MethodInfo mi, object[] args)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if(methodBinding == null)
            {
                throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
            }

            var operationAddress = methodBinding.Transports.OfType<IHttpTransport>().Select(o => o.OperationAddress).FirstOrDefault();

            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, operationAddress);

            AddTargetInstance(targetInstance, httpReq);
            AddSecurityKey(methodBinding, httpReq);

            var httpClientName = methodBinding.MethodBinder.OpenApiSpec.Title;
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Using client '{httpClientName}' to send data to remote host {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);
            var httpClientReq = new HttpRequestMessage();
            var headers = new Dictionary<string, IEnumerable<string>>();
            //await MethodHeadersTransformer(invocation.ServiceProvider, headers, invocation.SolidProxyInvocationConfiguration.MethodInfo);
            foreach (var additionalHeader in headers)
            {
                httpClientReq.Headers.Add(additionalHeader.Key, additionalHeader.Value);
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
