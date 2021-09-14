using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using System;
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
    public abstract class TransportHandler<TTransport> : ITransportHandler<TTransport> where TTransport : ITransport
    {
        /// <summary>
        /// The transport type
        /// </summary>
        public static readonly string TransportType = ITransportExtensions.GetTransportType(typeof(TTransport));

        public TransportHandler(ILogger<TransportHandler<TTransport>> logger, IMethodBinderStore methodBinderStore)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
        }

        protected ILogger Logger { get; }
        protected IMethodBinderStore MethodBinderStore { get; }

        string ITransportHandler.TransportType => TransportType;

        public virtual Task<object> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, object[] args, InvocationOptions invocationOptions)
        {
            if (invocationOptions == null)
            {
                var transport = methodBinding.Transports.First();
                invocationOptions = new InvocationOptions(transport.GetTransportType(), InvocationOptions.MessagePriorityNormal);
            }
            if (TransportType != invocationOptions.TransportType)
            {
                throw new ArgumentException("TransportType for invocation options does not match handler type.");
            }
            var mi = methodBinding.MethodInfo;
            var proxy = (ISolidProxy)serviceProvider.GetService(mi.DeclaringType);
            if (proxy == null) throw new Exception("Cannot resolve type for invocation " + mi.DeclaringType.FullName);
            return proxy.InvokeAsync(serviceProvider, this, mi, args, new Dictionary<string, object>
            {
                { typeof(InvocationOptions).FullName, invocationOptions }
            });
        }

        public virtual async Task<TResp> InvokeAsync<TResp>(IServiceProvider serviceProvider, IMethodBinding methodBinding, ITransport transport, object[] args, InvocationOptions invocationOptions)
        {
            if (methodBinding == null) throw new ArgumentNullException(nameof(methodBinding));
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, transport.OperationAddress);

            var cancellationToken = args.OfType<CancellationToken>().FirstOrDefault();
            await invocationOptions.PreInvokeCallback(httpReq);
            var httpResp = await InvokeAsync(serviceProvider, methodBinding, transport, httpReq, invocationOptions, cancellationToken);
            await invocationOptions.PostInvokeCallback(httpResp);

            var resp = methodBinding.ExtractResponse<TResp>(httpResp);
            return resp;
        }

        public Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            return InvokeAsync(serviceProvider, methodBinding, (TTransport)transport, httpReq, invocationOptions, cancellationToken);
        }

        public virtual void Configure(IMethodBinding methodBinding, TTransport transport)
        {

        }

        public abstract Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, TTransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken);
    }
}
