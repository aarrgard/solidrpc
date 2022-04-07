using SolidRpc.Abstractions.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.InternalServices
{
    /// <summary>
    /// Provides access to protected content
    /// </summary>
    public interface ISolidRpcProtectedResource
    {
        /// <summary>
        /// Constructs a protected resource from supplied resource string
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="expiryTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<byte[]> ProtectAsync(
            string resource,
            DateTimeOffset expiryTime,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Protectes a resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<byte[]> ProtectAsync(
            ProtectedResource resource,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Decrypts a protected resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ProtectedResource> UnprotectAsync(
            byte[] resource, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the protected content
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetProtectedContentAsync(
            byte[] resource, 
            string fileName,
            CancellationToken cancellationToken = default);
    }
}
