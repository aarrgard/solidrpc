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
    public class OpenApiSpecResolverDummy : ModelBase, IOpenApiSpecResolver
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OpenApiSpecResolverDummy(IOpenApiParser openApiParser) : base(null)
        {
            OpenApiParser = openApiParser;
        }

        public IOpenApiParser OpenApiParser { get; }

        /// <summary>
        /// Resolves the api spec with supplied name
        /// </summary>
        /// <param name="address"></param>
        /// <param name="openApiSpec"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public bool TryResolveApiSpec(string address, out IOpenApiSpec openApiSpec, string basePath = null)
        {
            throw new Exception("No resolver registered.");
        }
    }
}
