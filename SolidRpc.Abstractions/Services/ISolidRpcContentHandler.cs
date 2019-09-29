using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// The content handler uses the ISolidRpcContentStore to deliver static or proxied content.
    /// 
    /// This handler can be invoked from a configured proxy or mapped directly in a .Net Core Handler.
    /// </summary>
    public interface ISolidRpcContentHandler    {

        /// <summary>
        /// Returns the content for supplied path.
        /// 
        /// Note that the path is marked as optional(default value set). This is so that the parameter
        /// is placed in the query string instead of path.
        /// </summary>
        /// <param name="path">The path to get the content for</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetContent(string path = null, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Returns all the path prefixes. If content is added by using relative
        /// paths then all the assemly base paths are returned from this property.
        /// </summary>
        IEnumerable<string> PathPrefixes { get; }

        /// <summary>
        /// Returns all the path mappings.
        /// </summary>
        IEnumerable<NameValuePair> PathMappings { get; }
    }
}