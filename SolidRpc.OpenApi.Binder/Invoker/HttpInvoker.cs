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
using System.Linq;
using System.Linq.Expressions;
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
        public HttpInvoker(
            ILogger<Invoker<T>> logger, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, methodBinderStore, serviceProvider)
        {
        }

        protected override IHandler FilterHandlers(IEnumerable<IHandler> handlers, IMethodBinding binding)
        {
            return handlers.OfType<HttpHandler>().FirstOrDefault();
        }

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
            var methodBinding = GetMethodBinding(mi);
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
    }
}
