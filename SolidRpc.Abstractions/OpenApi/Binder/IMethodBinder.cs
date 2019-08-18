using SolidRpc.Abstractions.OpenApi.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// The method binder is responsible for binding MethodInfo structures to a swagger spec.
    /// </summary>
    public interface IMethodBinder
    {
        /// <summary>
        /// The open api spec that this binder gets its information from
        /// </summary>
        IOpenApiSpec OpenApiSpec { get; }

        /// <summary>
        /// The associated assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Returns all the mapped methods
        /// </summary>
        IEnumerable<IMethodBinding> MethodInfos { get; }

        /// <summary>
        /// Returns the method info from supplied specification.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        IMethodBinding GetMethodInfo(MethodInfo methodInfo, BaseUriTransformer baseUriTransformer);
    }
}
