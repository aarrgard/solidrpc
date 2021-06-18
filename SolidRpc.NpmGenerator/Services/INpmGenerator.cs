using System.Threading.Tasks;
using SolidRpc.NpmGenerator.Types;
using System.Threading;
namespace SolidRpc.NpmGenerator.Services {
    /// <summary>
    /// The npm generator
    /// </summary>
    public interface INpmGenerator {
        /// <summary>
        /// Creates types.ts file from the code namespace structure.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to create the types.ts file for.</param>
        /// <param name="cancellationToken"></param>
        Task<string> CreateTypesTs(
            string assemblyName,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to create an npm package for.</param>
        /// <param name="cancellationToken"></param>
        Task<NpmPackage> CreateNpmPackage(
            string assemblyName,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">The npm command to run</param>
        /// <param name="npmPackage">The npm package to compile</param>
        /// <param name="cancellationToken"></param>
        Task<NpmPackage> RunNpm(
            string command,
            NpmPackage npmPackage,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to create an npm package for.</param>
        /// <param name="cancellationToken"></param>
        Task<FileContent> CreateNpm(
            string assemblyName,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}