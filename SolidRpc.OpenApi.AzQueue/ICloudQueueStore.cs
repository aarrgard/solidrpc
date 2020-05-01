using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// Returns the cloud table
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        CloudBlobContainer GetCloudBlobContainer(string connectionName);

        /// <summary>
        /// Stores the supplied message as a blob if larger than 65k.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> StoreLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retreives the specified message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> RetreiveLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes the large message when processed
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
