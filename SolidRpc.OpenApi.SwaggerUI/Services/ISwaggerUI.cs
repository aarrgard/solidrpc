using SolidRpc.OpenApi.SwaggerUI.Types;
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetIndexHtml(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the swagger urls.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<SwaggerUrl>> GetSwaggerUrls(CancellationToken cancellationToken = default(CancellationToken));
    }
}