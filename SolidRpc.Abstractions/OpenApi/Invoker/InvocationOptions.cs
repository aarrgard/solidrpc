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
        public static readonly InvocationOptions Http = new InvocationOptions("Http", MessagePriorityNormal);

        /// <summary>
        /// The invocation options that uses the local implementation.
        /// </summary>

        public static readonly InvocationOptions Local = new InvocationOptions("Local", MessagePriorityNormal);

        /// <summary>
        /// The invocation options that uses the azure queue.
        /// </summary>

        public static readonly InvocationOptions AzQueue = new InvocationOptions("AzQueue", MessagePriorityNormal);

        /// <summary>
        /// The invocation options that uses the azure table.
        /// </summary>

        public static readonly InvocationOptions AzTable = new InvocationOptions("AzTable", MessagePriorityNormal);

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="transportType"></param>
        /// <param name="priority"></param>
        public InvocationOptions(string transportType, int priority)
        {
            TransportType = transportType;
            Priority = priority;
        }

        /// <summary>
        /// The preferred transport type. Defaults to "Http"
        /// </summary>
        public string TransportType { get; }

        /// <summary>
        /// The invocation priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Returns a copy of this instance with another priority.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public InvocationOptions SetPriority(int priority)
        {
            return new InvocationOptions(TransportType, priority);
        }
    }
}
