using System;

namespace SolidRpc.OpenApi.AspNetCore.Types
{
    /// <summary>
    /// Raised when the file content is not found
    /// </summary>
    public class FileContentNotFoundException : Exception
    {
        /// <summary>
        /// The file content
        /// </summary>
        public FileContentNotFoundException()
        {
            Data["HttpStatusCode"] = 404;
        }
    }
}
