using SolidRpc.Abstractions.Types.Code;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services.Code
{
    /// <summary>
    /// instance responsible for generating code structures
    /// </summary>
    public interface ICodeNamespaceGenerator
    {
        /// <summary>
        /// Creates a code namespace for supplied assembly name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CodeNamespace> CreateCodeNamespace(
            string assemblyName,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
