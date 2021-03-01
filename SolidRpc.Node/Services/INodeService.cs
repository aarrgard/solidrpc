using System.Threading.Tasks;
using SolidRpc.Node.Types;
using System.Threading;
using System.Collections.Generic;
using System;

namespace SolidRpc.Node.Services {
    /// <summary>
    /// The node service
    /// </summary>
    public interface INodeService
    {
        /// <summary>
        /// Lists all the available node modules
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<NodeModules>> ListNodeModulesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Runs the supplied javascript.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecution> ExecuteScriptAsync(Guid moduleId, string js, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Runs the supplied file. The file must be relativ to the modules dir.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecution> ExecuteFileAsync(Guid moduleId, string workingDir, string fileName, IEnumerable<string> args = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the node version
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetNodeVersionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}