using System.IO;

namespace SolidRpc.Node.Types
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public class NodeExecutionFile
    {
        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The file content
        /// </summary>
        public Stream Content { get; set; }
    }
}