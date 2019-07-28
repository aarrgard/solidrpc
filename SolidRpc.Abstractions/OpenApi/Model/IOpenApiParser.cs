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
        /// Creates a specification that contains the methods in supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IOpenApiSpec CreateSpecification(Type type);

        /// <summary>
        /// Creates a specification that contains the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IOpenApiSpec CreateSpecification(MethodInfo method);
    }
}
