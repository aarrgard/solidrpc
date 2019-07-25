using System.IO;

namespace SolidRpc.OpenApi.AzFunctions.Types
{
    /// <summary>
    /// Represents a file content
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// The file content.
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        public string  ContentType { get; set; }
    }
}
