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
        /// Writes the supplied openapi specification as a string
        /// </summary>
        /// <param name="openApiSpec"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        string WriteSwaggerSpec(IOpenApiSpec openApiSpec, bool formatted = true);

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
        
        /// <summary>
        /// Uses the serializer to clone the supplied node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        T CloneNode<T>(T node) where T : IOpenApiModelBase;
    }
}
