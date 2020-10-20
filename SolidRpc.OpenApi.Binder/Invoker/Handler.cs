using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Base class for the handlers
    /// </summary>
    public abstract class Handler : IHandler
    {
        private static readonly IEnumerable<IHttpRequestData> EmptyCookieList = new IHttpRequestData[0];
        private static ConcurrentDictionary<Type, Func<object, object>> Invokers = new ConcurrentDictionary<Type, Func<object, object>>();
        protected static string GetTransportType(Type type)
        {
            return type.Name.Substring(0, type.Name.Length - "Handler".Length);
        }

        public Handler(ILogger<Handler> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        protected ILogger Logger { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IMethodBinderStore MethodBinderStore => ServiceProvider.GetRequiredService<IMethodBinderStore>();

        public string TransportType => GetTransportType(GetType());

        public virtual Task<object> InvokeAsync(IMethodBinding methodBinding, object target, MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            var proxy = (ISolidProxy)target;
            return proxy.InvokeAsync(this, mi, args, new Dictionary<string, object>
            {
                { typeof(IHandler).FullName, this },
                { typeof(InvocationOptions).FullName, invocationOptions }
            });
        }

        public virtual async Task<TResp> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, object[] args, InvocationOptions invocationOptions)
        {
            if (methodBinding == null) throw new ArgumentNullException(nameof(methodBinding));
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, transport.OperationAddress);

            var cancellationToken = args.OfType<CancellationToken>().FirstOrDefault();
            await invocationOptions.PreInvokeCallback(httpReq);
            var httpResp = await InvokeAsync(methodBinding, transport, httpReq, invocationOptions, cancellationToken);
            await invocationOptions.PostInvokeCallback(httpResp);

            var resp = methodBinding.ExtractResponse<TResp>(httpResp);
            return resp;
        }

        public abstract Task<IHttpResponse> InvokeAsync(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken);
    }
}
