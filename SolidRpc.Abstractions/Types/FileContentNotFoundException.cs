﻿using System;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Raised when the file content is not found
    /// </summary>
    public class FileContentNotFoundException : Exception
    {
        /// <summary>
        /// The http status code
        /// </summary>
        public static readonly int HttpStatusCode = 404;
        private void Init()
        {
            Data["HttpStatusCode"] = HttpStatusCode;
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public FileContentNotFoundException()
        {
            Init();
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public FileContentNotFoundException(string message) : base(message)
        {
            Init();
        }
    }
}
