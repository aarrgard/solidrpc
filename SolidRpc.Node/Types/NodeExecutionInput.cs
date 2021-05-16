using System;

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
        /// The script to execute
        /// </summary>
        public string Js { get; set; }
    }
}
