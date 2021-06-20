using SolidRpc.Abstractions.Types.Code;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services.Code
{
    /// <summary>
    /// instance responsible for generating code structures
    /// </summary>
    public interface ITypescriptGenerator
    {
        /// <summary>
        /// Creates a types.ts file from supplied assembly name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> CreateTypesTsForAssemblyAsync(
            string assemblyName,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a types.ts file from supplied code namespace
        /// </summary>
        /// <param name="codeNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> CreateTypesTsForCodeNamespaceAsync(
            CodeNamespace codeNamespace,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
