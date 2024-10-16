﻿using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Interface that exposes functionality to invoke a method in an IoC container.
    /// </summary>
    public interface IMethodInvoker
    {
        /// <summary>
        /// The store that contains all the bindings
        /// </summary>
        IMethodBinderStore MethodBinderStore { get; }

        /// <summary>
        /// Invokes the supplied request.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="invocationSource"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource, 
            IHttpRequest request, 
            CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Invokes the supplied request.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="invocationSource"></param>
        /// <param name="request"></param>
        /// <param name="methodBindings"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource, 
            IHttpRequest request, 
            IEnumerable<IMethodBinding> methodBindings, 
            CancellationToken cancellation = default(CancellationToken));
    }
}
