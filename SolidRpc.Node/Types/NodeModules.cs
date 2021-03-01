using System;
using System.Collections.Generic;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents a set of node modules
    /// </summary>
    public class NodeModules
    {
        /// <summary>
        /// The id of the node module set
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The packages
        /// </summary>
        public IEnumerable<NodePackage> Packages { get; set; }
    }
}
