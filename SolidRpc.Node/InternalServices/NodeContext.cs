using System;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// A node execution context
    /// </summary>
    public class NodeContext
    {
        public NodeContext(
            Guid scope,
            string nodeExePath,
            string nodeWorkingDir,
            string nodeModulesDir)
        {
            Scope = scope;
            NodeExePath = nodeExePath;
            NodeWorkingDir = nodeWorkingDir;
            NodeModulesDir = nodeModulesDir;
        }
        public Guid Scope { get; }
        /// <summary>
        /// The node execution path
        /// </summary>
        public string NodeExePath { get; }

        /// <summary>
        /// The working path
        /// </summary>
        public string NodeWorkingDir { get; }

        /// <summary>
        /// The node modules directory
        /// </summary>
        public string NodeModulesDir { get; }
    }
}
