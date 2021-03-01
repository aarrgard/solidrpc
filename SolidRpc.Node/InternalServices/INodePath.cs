using System.Collections.Generic;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Interface that we use to resolve node paths
    /// </summary>
    public interface INodePath
    {
        /// <summary>
        /// Returns the node path
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNodeExePaths();
    }
}
