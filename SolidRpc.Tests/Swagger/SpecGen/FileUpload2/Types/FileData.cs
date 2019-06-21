﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.FileUpload2.Types
{
    /// <summary>
    /// Represents some file data
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The file content
        /// </summary>
        public Stream FileContent { get; set; }
    }
}