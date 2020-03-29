using Microsoft.WindowsAzure.Storage.Queue;

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
        CloudQueue GetQueueClient(string connectionName, string queueName);
    }
}
