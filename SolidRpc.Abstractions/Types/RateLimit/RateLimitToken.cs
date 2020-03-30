using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.Types.RateLimit
{
    /// <summary>
    /// Identifies a 
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
    }
}
