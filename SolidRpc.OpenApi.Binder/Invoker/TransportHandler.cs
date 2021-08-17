﻿using Microsoft.Extensions.DependencyInjection;
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

        public TransportHandler(ILogger<TransportHandler<TTransport>> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        protected ILogger Logger { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IMethodBinderStore MethodBinderStore => ServiceProvider.GetRequiredService<IMethodBinderStore>();

        string ITransportHandler.TransportType => TransportType;

        public virtual Task<object> InvokeAsync(IMethodBinding methodBinding, object target, MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            if (invocationOptions == null)
            {
                var transport = methodBinding.Transports.OrderBy(o => o.InvocationStrategy).First();
                invocationOptions = new InvocationOptions(transport.GetTransportType(), InvocationOptions.MessagePriorityNormal);
            }
            if (TransportType != invocationOptions.TransportType)
            {
                throw new ArgumentException("TransportType for invocation options does not match handler type.");
            }
            var proxy = (ISolidProxy)target;
            return proxy.InvokeAsync(this, mi, args, new Dictionary<string, object>
            {
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

        public Task<IHttpResponse> InvokeAsync(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            return InvokeAsync(methodBinding, (TTransport)transport, httpReq, invocationOptions, cancellationToken);
        }

        public virtual void Configure(IMethodBinding methodBinding, TTransport transport)
        {

        }

        public abstract Task<IHttpResponse> InvokeAsync(IMethodBinding methodBinding, TTransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken);
    }
}