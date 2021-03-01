using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents a node module
    /// </summary>
    public class NodePackage
    {
        /// <summary>
        /// The node module name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The module version
        /// </summary>
        public string Version { get; set; }
    }
}
