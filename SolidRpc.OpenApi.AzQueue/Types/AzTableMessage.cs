using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzQueue.Types
{
    /// <summary>
    /// Represents a table message
    /// </summary>
    public class AzTableMessage
    {
        /// <summary>
        /// The connection name
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// The partition key
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// The row key
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// The status of the message
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The payload of the message
        /// </summary>
        public string Message { get; set; }
    }
}
