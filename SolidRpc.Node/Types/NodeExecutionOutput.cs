using System.Collections.Generic;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents the result from a node execution.
    /// </summary>
    public class NodeExecutionOutput
    {
        /// <summary>
        /// The exit code.
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// The result from the execution
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// The result from the std.out.
        /// </summary>
        public string Out { get; set; }

        /// <summary>
        /// The result from the std.err.
        /// </summary>
        public string Err { get; set; }

        /// <summary>
        /// The result files
        /// </summary>
        public IEnumerable<NodeExecutionFile> ResultFiles { get; set; }
    }
}
