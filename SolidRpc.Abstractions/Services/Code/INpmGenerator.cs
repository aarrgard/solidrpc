using System.Threading.Tasks;
using System.Threading;
using SolidRpc.Abstractions.Types.Code;
using SolidRpc.Abstractions.Types;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.Services.Code
{
    /// <summary>
    /// The npm generator
    /// </summary>
    public interface INpmGenerator {
        /// <summary>
        /// Returns the files that should be stored in the node_modules directory
        /// </summary>
        /// <param name="assemblyNames">The name of the assemblies to create an npm package for.</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<NpmPackage>> CreateNpmPackage(
            IEnumerable<string> assemblyNames,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns a zip containing the npm packages. This zip can be exploded in the node_modules directory.
        /// </summary>
        /// <param name="assemblyNames">The name of the assembly to create an npm package for.</param>
        /// <param name="cancellationToken"></param>
        Task<FileContent> CreateNpmZip(
            IEnumerable<string> assemblyNames,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}