using System;

namespace SolidRpc.Abstractions.Types.RateLimit
{
    /// <summary>
    /// Token returned from a resource request
    /// </summary>
    public class RateLimitToken
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// The unique id of this token. This guid may be empty if no token
        /// was issued from the service.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The time when the token expires
        /// </summary>
        public DateTimeOffset Expires { get; set; }
    }
}
