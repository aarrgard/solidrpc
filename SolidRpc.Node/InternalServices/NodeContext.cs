using SolidRpc.Node.Services;
using System;

namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// A node execution context
    /// </summary>
    public class NodeContext
    {
        public NodeContext(
            INodeModuleResolver resolver,
            string nodeExePath,
            string nodeWorkingDir,
            string nodeModulesDir)
        {
            Resolver = resolver;
            NodeExePath = nodeExePath;
            NodeWorkingDir = nodeWorkingDir;
            NodeModulesDir = nodeModulesDir;
        }

        /// <summary>
        /// The resolver
        /// </summary>
        public INodeModuleResolver Resolver { get; }

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
