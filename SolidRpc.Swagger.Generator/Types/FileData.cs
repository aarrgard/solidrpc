using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidRpc.Swagger.Generator.Types
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// The file content
        /// </summary>
        public Stream FileStream { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string Filename { get; set; }
    }
}
