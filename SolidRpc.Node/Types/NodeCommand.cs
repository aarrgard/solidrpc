using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents a node command
    /// </summary>
    public class NodeCommand
    {
        /// <summary>
        /// The command id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The script
        /// </summary>
        public string Script { get; set; }
    }
}
