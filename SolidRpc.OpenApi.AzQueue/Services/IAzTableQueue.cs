using SolidRpc.OpenApi.AzQueue.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzQueue.Services
{
    /// <summary>
    /// Definitions to access the table queues
    /// </summary>
    public interface IAzTableQueue
    {
        /// <summary>
        /// Returns the table queue settings.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<AzTableQueueSettings>> GetSettingsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates the supplied settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AzTableQueueSettings> UpdateSettings(AzTableQueueSettings settings, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Dispatches messages to the queue. If there are any messages pending.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task DoScheduledScanAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Dispatches messages to the queue. If there are any messages pending.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task DispatchMessageAsync(string connectionName, string queueName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessMessageAsync(string connectionName, string partitionKey, string rowKey, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Flaggs all the messages that are in error state to pending
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FlagErrorMessagesAsPending(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sends a test message.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendTestMessageAync(Stream payload, int messageCount = 1, bool raiseException = false, int messagePriority = 5, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sends a test message.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessTestMessage(Stream payload, bool raiseException, CancellationToken cancellationToken = default(CancellationToken));
    }
}
