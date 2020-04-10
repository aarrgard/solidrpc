using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Types;
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
        public Handler(ILogger<Handler> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        protected ILogger Logger { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IMethodBinderStore MethodBinderStore => ServiceProvider.GetRequiredService<IMethodBinderStore>();

        public abstract ITransport GetTransport(IEnumerable<ITransport> transports);

        public virtual Task<TResp> InvokeAsync<TResp>(MethodInfo mi, object[] args)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            return InvokeAsync<TResp>(methodBinding, GetTransport(methodBinding.Transports), args);
        }

        public virtual async Task<TResp> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, object[] args)
        {
            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, transport.OperationAddress);

            AddSecurityKey(methodBinding, httpReq);
            //AddTargetInstance(methodBinding, httpReq);

            var cancellationToken = args.OfType<CancellationToken>().FirstOrDefault();
            var httpResp = await InvokeAsync<TResp>(methodBinding, transport, httpReq, cancellationToken);

            var resp = methodBinding.ExtractResponse<TResp>(httpResp);
            return resp;
        }

        protected abstract Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, CancellationToken cancellationToken);

        protected void AddSecurityKey(IMethodBinding methodBinding, IHttpRequest httpReq)
        {
            //
            // Add security key header
            //
            var securityKey = methodBinding.SecurityKey;
            if (securityKey != null)
            {
                var headers = httpReq.Headers.ToList();
                headers.Add(new SolidHttpRequestDataString("text/plain", securityKey.Value.Key, securityKey.Value.Value));
                httpReq.Headers = headers;
            }
        }

        protected void AddTargetInstance(SolidRpcHostInstance targetInstance, IHttpRequest httpReq)
        {
            if (targetInstance == null)
            {
                return;
            }

            var newCookies = EmptyCookieList;

            // add the cookies if set
            if (targetInstance.HttpCookies != null)
            {
                newCookies = targetInstance.HttpCookies.Select(o => new SolidHttpRequestDataString("text/plain", "Cookie", $"{o.Key}={o.Value}"));
            }

            // add the "x-solidrpchosttarget"
            newCookies = newCookies.Union(new[] { new SolidHttpRequestDataString("text/plain", "X-SolidRpcTargetHost", targetInstance.HostId.ToString()) });

            httpReq.Headers = httpReq.Headers.Union(newCookies).ToList();
        }

    }
}
