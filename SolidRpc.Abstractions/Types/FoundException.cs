using System;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Raised when we want to send a redirect
    /// </summary>
    public class FoundException : Exception
    {
        /// <summary>
        /// The http status code
        /// </summary>
        public static readonly int HttpStatusCode = 302;

        private void Init()
        {
            Data["HttpStatusCode"] = HttpStatusCode;
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public FoundException(Uri uri)
        {
            Init();
            Data["HttpLocation"] = uri;
        }
    }
}
