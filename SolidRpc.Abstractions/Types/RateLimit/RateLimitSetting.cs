using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.Types.RateLimit
{
    /// <summary>
    /// Specifies the rate limit settings for the specified resource
    /// </summary>
    public class RateLimitSetting
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// If set specifies the maximum number of concurrent calls
        /// there can be to the resource
        /// </summary>
        public int? MaxConcurrentCalls { get; set; }
    }
}
