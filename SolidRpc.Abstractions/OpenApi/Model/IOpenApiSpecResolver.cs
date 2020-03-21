using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// This interface defines the logic assocated
    /// with resolving referenced specifications.
    /// </summary>
    public interface IOpenApiSpecResolver
    {
        /// <summary>
        /// Returns the parser that parses the resolved specifications.
        /// </summary>
        IOpenApiParser OpenApiParser { get; }

        /// <summary>
        /// Resolves the specification at supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="openApiSpec"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        bool TryResolveApiSpec(string address, out IOpenApiSpec openApiSpec, string basePath = null);
    }
}
