using System;
using System.Collections.Generic;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents the result from a node execution.
    /// </summary>
    public class NodeExecutionInput
    {
        /// <summary>
        /// The module to execute the script in
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// The files to write to the working dir prior to execution.
        /// </summary>
        public IEnumerable<NodeExecutionFile> InputFiles { get; set; }

        /// <summary>
        /// The script or file to execute. If no args are supplied then the supplied
        /// text is treated as a script. 
        /// </summary>
        public string Js { get; set; }

        /// <summary>
        /// The arguments to supply to the script
        /// </summary>
        public string[] Args { get; set; }
    }
}
