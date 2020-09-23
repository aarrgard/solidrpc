using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
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

        public virtual Task<TResp> InvokeAsync<TResp>(MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            var transport = methodBinding.Transports.FirstOrDefault(o => o.TransportType == TransportType);
            if (transport == null) throw new Exception($"Transport({TransportType}) not configured for method {mi.DeclaringType.FullName}.{mi.Name}. Configured transports are {string.Join(",", methodBinding.Transports.Select(o => o.TransportType))}"); 
            return InvokeAsync<TResp>(methodBinding, transport, args, invocationOptions);
        }

        public virtual async Task<TResp> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, object[] args, InvocationOptions invocationOptions)
        {
            if (methodBinding == null) throw new ArgumentNullException(nameof(methodBinding));
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, transport.OperationAddress);

            AddSecurityKey(methodBinding, httpReq);
            //AddTargetInstance(methodBinding, httpReq);

            var cancellationToken = args.OfType<CancellationToken>().FirstOrDefault();
            await invocationOptions.PreInvokeCallback(httpReq);
            var httpResp = await InvokeAsync<TResp>(methodBinding, transport, httpReq, invocationOptions, cancellationToken);
            await invocationOptions.PostInvokeCallback(httpResp);

            var resp = methodBinding.ExtractResponse<TResp>(httpResp);
            return resp;
        }

        protected void AddSecurityKey(IMethodBinding methodBinding, IHttpRequest httpReq)
        {
            //
            // Add security key header
            //
            var securityKey = methodBinding.GetSolidProxyConfig<ISecurityKeyConfig>()?.SecurityKey;
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

        public abstract Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken);
    }
}
