using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
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
        /// <param name="pathPrefix"></param>
        void AddContent(Assembly assembly, string packagePath, string pathPrefix);
        
        /// <summary>
        /// These are all the registered paths.
        /// </summary>
        IEnumerable<string> PathPrefixes { get; }

        /// <summary>
        /// Returns the content for supplied file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetStaticContent(string path, CancellationToken cancellationToken = default(CancellationToken));
    }
}