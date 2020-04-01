using System;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a solid rpc host.
    /// </summary>
    public class SolidRpcHostInstance
    {
        /// <summary>
        /// The unique id of this host. This id is regenerated every time a 
        /// new memory context is created
        /// </summary>
        public Guid HostId { get; set; }

        /// <summary>
        /// The time this host was started.
        /// </summary>
        public DateTimeOffset Started { get; set; }

        /// <summary>
        /// The last time this host was alive. This field is set(and returned) when a client
        /// invokes the ISolidRpcHost.GetHostInstance.
        /// </summary>
        public DateTimeOffset LastAlive { get; set; }

        /// <summary>
        /// The cookie to set in order to reach this host
        /// </summary>
        public IDictionary<string, string> HttpCookies { get; set; }
    }
}
