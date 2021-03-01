using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Factory for creating node processes.
    /// </summary>
    public interface INodeProcessFactory
    {
        /// <summary>
        /// Returns a new node execution process
        /// </summary>
        /// <returns></returns>
        Task<INodeProcess> CreateNodeProcessAsync(Guid scope, string workingDir = null, CancellationToken cancellation = default);
    }
}
