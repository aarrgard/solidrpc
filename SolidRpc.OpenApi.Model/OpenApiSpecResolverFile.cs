using SolidRpc.Abstractions.OpenApi.Model;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Resolves specifications that are embedded in an assembly.
    /// </summary>
    public class OpenApiSpecResolverFile : ModelBase, IOpenApiSpecResolver
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OpenApiSpecResolverFile(IOpenApiParser openApiParser) : base(null)
        {
            OpenApiParser = openApiParser ?? throw new System.ArgumentNullException(nameof(openApiParser));
            OpenApiSpecs = new ConcurrentDictionary<string, IOpenApiSpec>();
        }

        private ConcurrentDictionary<string, IOpenApiSpec> OpenApiSpecs { get; }
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
            openApiSpec = OpenApiSpecs.GetOrAdd(address, _ => {
                throw new NotImplementedException();
            });
            return openApiSpec != null;
        }
    }
}
