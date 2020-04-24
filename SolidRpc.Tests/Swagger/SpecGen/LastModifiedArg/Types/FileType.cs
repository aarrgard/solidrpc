using System;
using System.IO;

namespace SolidRpc.Tests.Swagger.SpecGen.LastModifiedArg.Types
{
    /// <summary>
    /// ComplexType2
    /// </summary>
    public class FileType
    {
        /// <summary>
        /// The content
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// The ETag.
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }
    }
}
