﻿using SolidRpc.OpenApi.SwaggerUI.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.SwaggerUI.Services
{
    /// <summary>
    /// The swagger config
    /// </summary>
    public interface ISwaggerUI
    {
        /// <summary>
        /// Returns the index.html file.
        /// </summary>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetIndexHtml(
            bool onlyImplemented = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the oauth2-redirect file
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetOauth2RedirectHtml(
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the swagger urls.
        /// </summary>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<SwaggerUrl>> GetSwaggerUrls(
            bool onlyImplemented = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the openapi spec for supplied assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="openApiSpecResolverAddress">The name of the openapi resource</param>
        /// <param name="onlyImplemented"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetOpenApiSpec(
            string assemblyName, 
            string openApiSpecResolverAddress,
            bool onlyImplemented = true,
            CancellationToken cancellationToken = default(CancellationToken));

    }
}