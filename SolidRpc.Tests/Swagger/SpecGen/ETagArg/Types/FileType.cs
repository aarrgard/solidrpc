using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.ETagArg.Types
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
        public string ETag { get; set; }
    }
}
