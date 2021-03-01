using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents a response to a command
    /// </summary>
    public class NodeResponse
    {
        /// <summary>
        /// The command id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The script
        /// </summary>
        public string Result { get; set; }
    }
}
