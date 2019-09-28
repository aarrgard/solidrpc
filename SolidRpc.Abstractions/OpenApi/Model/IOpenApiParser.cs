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
        /// <param name="specResolver">The spec resolver.</param>
        /// <param name="address">the address of the json to parse.</param>
        /// <param name="json">The json to parse</param>
        /// <returns></returns>
        IOpenApiSpec ParseSpec(IOpenApiSpecResolver specResolver, string address, string json);

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
