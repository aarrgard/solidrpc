using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Contains additional invocation options.
    /// </summary>
    public class InvocationOptions
    {
        /// <summary>
        /// The high message prio
        /// </summary>
        public static readonly int MessagePriorityHigh = 3;
        /// <summary>
        /// The normal message prio
        /// </summary>
        public static readonly int MessagePriorityNormal = 5;
        /// <summary>
        /// The low message prio
        /// </summary>
        public static readonly int MessagePriorityLow = 7;

        /// <summary>
        /// The invocation options that uses the http protocol
        /// </summary>
        public static readonly InvocationOptions Http = new InvocationOptions() { TransportType = "Http"};
        
        /// <summary>
        /// The invocation options that uses the local implementation.
        /// </summary>

        public static readonly InvocationOptions Local = new InvocationOptions() { TransportType = "Local" };

        /// <summary>
        /// The preferred transport type. Defaults to "Http"
        /// </summary>
        public string TransportType { get; set; } = "Http";

        /// <summary>
        /// The invocation priority.
        /// </summary>
        public int Priority { get; set; } = MessagePriorityNormal;
    }
}
