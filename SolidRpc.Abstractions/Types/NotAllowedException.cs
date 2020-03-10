using System;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Raised when the the method invocation is not allowed
    /// </summary>
    public class UnauthorizedException : Exception
    {
        /// <summary>
        /// The http status code
        /// </summary>
        public static readonly int HttpStatusCode = 401;

        private void Init()
        {
            Data["HttpStatusCode"] = 401;
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public UnauthorizedException()
        {
            Init();
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public UnauthorizedException(string message) : base(message)
        {
            Init();
        }
    }
}
