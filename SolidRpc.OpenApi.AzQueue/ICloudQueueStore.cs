using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace SolidRpc.OpenApi.AzQueue
{
    /// <summary>
    /// A store for the queue clients
    /// </summary>
    public interface ICloudQueueStore
    {
        /// <summary>
        /// Returns the cloud queue to access the queue at a specific connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        CloudQueue GetCloudQueue(string connectionName, string queueName);

        /// <summary>
        /// Returns the cloud table
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        CloudTable GetCloudTable(string connectionName);
    }
}
