using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.NpmGenerator.Types
{
    /// <summary>
    /// Represents the result from a node execution.
    /// </summary>
    public class NodeExecution
    {
        /// <summary>
        /// The exit code. Null means that the process is still running.
        /// </summary>
        public int? ExitCode { get; set; }

        /// <summary>
        /// The result from the std.out.
        /// </summary>
        public string Out { get; set; }

        /// <summary>
        /// The result from the std.err.
        /// </summary>
        public string Err { get; set; }
    }
}
