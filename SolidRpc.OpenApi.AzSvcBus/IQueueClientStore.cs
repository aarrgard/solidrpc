using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzSvcBus
{
    /// <summary>
    /// A store for the queue clients
    /// </summary>
    public interface IQueueClientStore
    {
        /// <summary>
        /// Returns the queue client to access the queue at a specific connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        QueueClient GetQueueClient(string connectionName, string queueName);
    }
}
