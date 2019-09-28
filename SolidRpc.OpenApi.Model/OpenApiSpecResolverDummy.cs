using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model;
using System;

[assembly: SolidRpcAbstractionProvider(typeof(IOpenApiSpecResolver), typeof(OpenApiSpecResolverAssembly))]
namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Resolves specifications that are embedded in an assembly.
    /// </summary>
    public class OpenApiSpecResolverDummy : IOpenApiSpecResolver
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly IOpenApiSpecResolver Instance = new OpenApiSpecResolverDummy();

        /// <summary>
        /// Resolves the api spec with supplied name
        /// </summary>
        /// <param name="address"></param>
        /// <param name="openApiSpec"></param>
        /// <returns></returns>
        public bool TryResolveApiSpec(string address, out IOpenApiSpec openApiSpec)
        {
            throw new Exception("No resolver registered.");
        }
    }
}
