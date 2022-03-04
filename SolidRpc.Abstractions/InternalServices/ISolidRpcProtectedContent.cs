using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.InternalServices
{
    /// <summary>
    /// Provides access to protected content
    /// </summary>
    public interface ISolidRpcProtectedContent
    {
        /// <summary>
        /// Returns the protected content for supplied resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the content for the supplied resource.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken);

        /// <summary>
        /// Constructs a list of protected resurce strings from supplied content paths
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<byte[]>> CreateProtectedResourceStrings(IEnumerable<string> content, CancellationToken cancellationToken);


    }
}
