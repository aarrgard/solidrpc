using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Base interface for the invokers
    /// </summary>
    public interface IInvoker
    {
        /// <summary>
        /// Returns the binding for supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding(MethodInfo methodInfo);

        /// <summary>
        /// Invokes the specified method with supplied arguments.
        /// </summary>
        /// <param name="invocationOptions"></param>
        /// <param name="methodInfo"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> InvokeAsync(MethodInfo methodInfo, IEnumerable<object> args, Func<InvocationOptions, InvocationOptions> invocationOptions = null);
    }

    /// <summary>
    /// Base interface for the invokers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInvoker<T> : IInvoker where T:class
    {
        /// <summary>
        /// Invokes an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding(Expression<Action<T>> action);

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding<TResult>(Expression<Func<T, TResult>> func);

        /// <summary>
        /// Returns the uri where we invoke the specified method.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="includeQueryString"></param>
        /// <returns></returns>
        Task<Uri> GetUriAsync(Expression<Action<T>> action, bool includeQueryString = true);

        /// <summary>
        /// Returns the uri where we invoke the specified method.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="includeQueryString"></param>
        /// <returns></returns>
        Task<Uri> GetUriAsync<TRes>(Expression<Func<T, TRes>> func, bool includeQueryString = true);

        /// <summary>
        /// Invokes an action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="invocationOptions"></param>
        /// <returns></returns>
        Task InvokeAsync(Expression<Action<T>> action, Func<InvocationOptions, InvocationOptions> invocationOptions = null);

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="func"></param>
        /// <param name="invocationOptions"></param>
        /// <returns></returns>
        TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func, Func<InvocationOptions, InvocationOptions> invocationOptions = null);
    }
}
