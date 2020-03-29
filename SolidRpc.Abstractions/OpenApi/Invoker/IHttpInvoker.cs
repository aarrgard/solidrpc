using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Uses the http stack to invoke the call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHttpInvoker<T> : IInvoker<T> where T : class
    {
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
        /// <returns></returns>
        Task<Uri> GetUriAsync<TRes>(Expression<Func<T,TRes>> func, bool includeQueryString = true);
    }
}
