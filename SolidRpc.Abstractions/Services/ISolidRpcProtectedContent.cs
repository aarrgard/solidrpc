using SolidRpc.Abstractions.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Provides access to protected content
    /// </summary>
    public interface ISolidRpcProtectedContent
    {
        /// <summary>
        /// Returns the content for the supplied resource.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken = default);
    }
}
