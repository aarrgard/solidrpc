using System;
using System.IO;

namespace SolidRpc.Abstractions.Types
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
        /// The content charset
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The last modified date of the resource.
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
