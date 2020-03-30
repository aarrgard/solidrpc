using System;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Raised when the rate limit has been exceeded
    /// </summary>
    public class RateLimitExceededException : Exception
    {
        /// <summary>
        /// The http status code
        /// </summary>
        public static readonly int HttpStatusCode = 429;
        private void Init()
        {
            Data["HttpStatusCode"] = HttpStatusCode;
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public RateLimitExceededException()
        {
            Init();
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public RateLimitExceededException(string message) : base(message)
        {
            Init();
        }
    }
}
