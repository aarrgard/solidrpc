using System;

namespace SolidRpc.OpenApi.AzQueue.Types
{
    /// <summary>
    /// The settings for the queue
    /// </summary>
    public class AzTableQueueSettings
    {
        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The maximum concurrent calls.
        /// </summary>
        public int MaxConcurrentCalls { get; set; }

        /// <summary>
        /// The timeout for the messages. Messages that have not been
        /// removed from the queue after this amount of time will
        /// be dispatched to the queue again.
        /// </summary>
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 15, 0);
    }
}
