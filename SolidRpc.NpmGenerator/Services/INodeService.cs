using System.Threading.Tasks;
using SolidRpc.NpmGenerator.Types;
using System.Threading;
namespace SolidRpc.NpmGenerator.Services {
    /// <summary>
    /// The npm generator
    /// </summary>
    public interface INodeService
    {
        /// <summary>
        /// Runs the supplied javascript.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecution> ExecuteJSInDebugger(string js, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Runs the supplied javascript.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecution> ExecuteJSAsync(string js, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the node version
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetNodeVersionAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Downloads the specified package so that it is available to the executed scripts.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="version"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadPackageAsync(string package, string version, CancellationToken cancellationToken = default(CancellationToken));
    }
}