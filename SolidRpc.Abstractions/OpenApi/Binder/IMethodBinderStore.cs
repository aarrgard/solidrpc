using SolidRpc.Abstractions.OpenApi.Model;
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
        /// Returns the open api spec resolver for supplied assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IOpenApiSpecResolver GetOpenApiSpecResolver(Assembly assembly);

        /// <summary>
        /// Returns all the method binders.
        /// </summary>
        IEnumerable<IMethodBinder> MethodBinders { get; }

        /// <summary>
        /// Returns the method info for supplied open api spec and reflected method
        /// </summary>
        /// <param name="openApiSpec">The openapi spec to use. If null we search the assembly.</param>
        /// <param name="localApi">Does the method have a local implementation</param>
        /// <param name="methodInfo">The method to creata a binding for</param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        IMethodBinding CreateMethodBinding(
            string openApiSpec, 
            bool localApi, 
            MethodInfo methodInfo,
            MethodAddressTransformer baseUriTransformer = null);

        /// <summary>
        /// Returns the uri to invoke the supplied method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeQueryString">Shoukd the query string be included</param>
        /// <returns></returns>
        Task<Uri> GetUrlAsync<T>(Expression<Action<T>> expression, bool includeQueryString = true);

        /// <summary>
        /// Returns the method info for the matching expressiong.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding<T>(Expression<Action<T>> expression);

        /// <summary>
        /// Returns the method info for the supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        IMethodBinding GetMethodBinding(MethodInfo methodInfo);
    }
}
