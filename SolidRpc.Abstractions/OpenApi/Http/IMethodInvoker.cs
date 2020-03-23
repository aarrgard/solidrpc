using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(IHttpRequest request, CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Invokes the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="methodInfo"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(IHttpRequest request, IMethodBinding methodInfo, CancellationToken cancellation = default(CancellationToken));
    }
}
