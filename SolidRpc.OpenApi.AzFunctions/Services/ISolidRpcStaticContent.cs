using SolidRpc.OpenApi.AzFunctions.Types;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
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
        /// <param name="packageParh"></param>
        /// <param name="absolutePath"></param>
        void AddContent(Assembly assembly, string packageParh, string absolutePath);

        /// <summary>
        /// Returns the content for supplied file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetStaticContent(string path, CancellationToken cancellationToken = default(CancellationToken));
    }
}