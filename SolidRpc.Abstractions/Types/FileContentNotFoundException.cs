using System;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Raised when the file content is not found
    /// </summary>
    public class FileContentNotFoundException : Exception
    {
        private void Init()
        {
            Data["HttpStatusCode"] = 404;
        }

        /// <summary>
        /// The file content
        /// </summary>
        public FileContentNotFoundException()
        {
            Init();
        }

        /// <summary>
        /// The file content
        /// </summary>
        public FileContentNotFoundException(string message) : base(message)
        {
            Init();
        }
    }
}
