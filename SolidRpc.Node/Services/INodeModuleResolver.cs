using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.Services
{
    /// <summary>
    /// Interface that can be implemented in order to provide 
    /// a set of node modules.
    /// </summary>
    public interface INodeModuleResolver
    {
        /// <summary>
        /// The module id
        /// </summary>
        Guid ModuleId { get; }

        /// <summary>
        /// Returns the node modules
        /// </summary>
        /// <returns></returns>
        Task ExplodeNodeModulesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken = default);
    }
}
