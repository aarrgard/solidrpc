using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Represents a node process
    /// </summary>
    public interface INodeProcess : IDisposable
    {
        /// <summary>
        /// Executes the supplied js script.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> ExecuteScriptAsync<T>(string js, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the supplied js script.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecutionOutput> ExecuteScriptAsync(string js, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the supplied js script.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NodeExecutionOutput> ExecuteFileAsync(string fileName, IEnumerable<string> args = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the version.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetVersionAsync(CancellationToken cancellationToken);
    }
}