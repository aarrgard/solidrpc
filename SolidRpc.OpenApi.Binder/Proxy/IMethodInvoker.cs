﻿using SolidRpc.OpenApi.Binder.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Proxy
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
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(IHttpRequest request, CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Invokes the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(IHttpRequest request, IMethodInfo methodInfo, CancellationToken cancellation = default(CancellationToken));
    }
}
