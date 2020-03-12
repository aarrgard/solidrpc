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

        /// <summary>
        /// Invokes the specified method with supplied arguments. If there is a SecurityKey specified for the method 
        /// it is passed along as an invocation parameter.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="args"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<object> InvokeInternalAsync(MethodInfo methodInfo, IEnumerable<object> args, CancellationToken cancellation = default(CancellationToken));
    }

    /// <summary>
    /// Invokes a method on an object in the service provider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMethodInvoker<T> : IMethodInvoker
    {
        /// <summary>
        /// Invokes the specified method with supplied arguments. If there is a SecurityKey specified for the method 
        /// it is passed along as an invocation parameter.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task InvokeInternalAsync<TResult>(Expression<Action<T>> action, CancellationToken cancellation = default(CancellationToken));
        
        /// <summary>
        /// Invokes the specified method with supplied arguments. If there is a SecurityKey specified for the method 
        /// it is passed along as an invocation parameter.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        TResult InvokeInternalAsync<TResult>(Expression<Func<T, TResult>> func, CancellationToken cancellation = default(CancellationToken));

    }
}
