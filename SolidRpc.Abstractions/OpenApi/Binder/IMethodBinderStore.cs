using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// A store that contains bindings for an assebly and open api configuration.
    /// </summary>
    public interface IMethodBinderStore
    {
        /// <summary>
        /// Returns all the method binders.
        /// </summary>
        IEnumerable<IMethodBinder> MethodBinders { get; }

        /// <summary>
        /// Returns the method info for supplied open api spec and reflected method
        /// </summary>
        /// <param name="baseUriTransformer"></param>
        /// <param name="openApiSpec"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        IMethodBinding GetMethodInfo(
            string openApiSpec, 
            MethodInfo methodInfo,
            BaseUriTransformer baseUriTransformer = null);

        /// <summary>
        /// Returns the uri to invoke the supplied method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<Uri> GetUrlAsync<T>(Expression<Action<T>> expression);

        /// <summary>
        /// Returns the method info for the matching expressiong.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding<T>(Expression<Action<T>> expression);
    }
}
