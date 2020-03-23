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
        /// Invokes the specified method with supplied arguments.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> InvokeAsync(MethodInfo methodInfo, IEnumerable<object> args);
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
        Task InvokeAsync(Expression<Action<T>> action);

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func);
    }
}
