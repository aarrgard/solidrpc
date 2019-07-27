using SolidRpc.Abstractions.Types;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Accesses the static content
    /// </summary>
    public interface ISolidRpcStaticContent
    {
        /// <summary>
        /// Adds a content mapping
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="packagePath"></param>
        /// <param name="absolutePath"></param>
        void AddContent(Assembly assembly, string packagePath, string absolutePath);

        /// <summary>
        /// Returns the content for supplied file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetStaticContent(string path, CancellationToken cancellationToken = default(CancellationToken));
    }
}