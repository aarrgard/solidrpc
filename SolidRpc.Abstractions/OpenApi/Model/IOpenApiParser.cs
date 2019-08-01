using System;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Interface used to handle open api specs
    /// </summary>
    public interface IOpenApiParser
    {
        /// <summary>
        /// Parses upplied json to an open api spec.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        IOpenApiSpec ParseSpec(string json);

        /// <summary>
        /// Creates a specification that contains the methods in supplied types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IOpenApiSpec CreateSpecification(params Type[] types);

        /// <summary>
        /// Creates a specification that contains the specified method.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        IOpenApiSpec CreateSpecification(params MethodInfo[] methods);
    }
}
