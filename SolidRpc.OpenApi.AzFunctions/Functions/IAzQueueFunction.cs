using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    /// <summary>
    /// Represents a timer function
    /// </summary>
    public interface IAzQueueFunction : IAzFunction
    {
        /// <summary>
        /// The queue name
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// The connection
        /// </summary>
        string Connection { get; set; }
    }
}
