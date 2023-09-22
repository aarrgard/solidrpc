using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Linq;
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

        public virtual Task<object> InvokeAsync(
            IServiceProvider serviceProvider, 
            IMethodBinding methodBinding, 
            object[] args)
        {
            var transport = methodBinding.Transports.Single(o => o.GetTransportType() == TransportType);
            var mi = methodBinding.MethodInfo;
            var proxy = (ISolidProxy)serviceProvider.GetService(mi.DeclaringType);
            if (proxy == null) throw new Exception("Cannot resolve type for invocation " + mi.DeclaringType.FullName);
            return proxy.InvokeAsync(serviceProvider, this, mi, args);
        }

        public virtual async Task<TResp> InvokeAsync<TResp>(
            IServiceProvider serviceProvider, 
            IMethodBinding methodBinding, 
            ITransport transport, 
            object[] args)
        {
            if (methodBinding == null) throw new ArgumentNullException(nameof(methodBinding));
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            var httpReq = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(httpReq, args, transport.OperationAddress);

            var cancellationToken = args.OfType<CancellationToken>().FirstOrDefault();
            var invocationOptions = InvocationOptions.GetOptions(methodBinding.MethodInfo);
            await invocationOptions.PreInvokeCallback(httpReq);
            var httpResp = await InvokeAsync(serviceProvider, methodBinding, transport, httpReq, cancellationToken);
            await invocationOptions.PostInvokeCallback(httpResp);

            var resp = methodBinding.ExtractResponse<TResp>(httpResp);
            return resp;
        }

        public Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, CancellationToken cancellationToken)
        {
            return InvokeAsync(serviceProvider, methodBinding, (TTransport)transport, httpReq, cancellationToken);
        }

        public virtual void Configure(IMethodBinding methodBinding, TTransport transport)
        {

        }

        public abstract Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, TTransport transport, IHttpRequest httpReq, CancellationToken cancellationToken);
    }
}
