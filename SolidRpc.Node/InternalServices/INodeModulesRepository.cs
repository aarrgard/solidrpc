using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Represents the node module repository
    /// </summary>
    public interface INodeModulesRepository
    {
        /// <summary>
        /// Returns the node modules
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NodeModules>> GetNodeModulesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the path to the node module for supplied id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetNodeModulePathAsync(Guid moduleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the module resolver for supplie module id.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        Task<INodeModuleResolver> GetNodeModuleResolverAsync(Guid moduleId, CancellationToken cancellationToken = default);
    }
}
