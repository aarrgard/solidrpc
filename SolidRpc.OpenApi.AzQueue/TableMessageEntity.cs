using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace SolidRpc.OpenApi.AzQueue
{
    public class TableMessageEntity : TableEntity
    {

        public static string CreatePartitionKey(string queueName)
        {
            return queueName.Replace('/', '-');
        }

        public const string StatusSettings= "Settings";
        public const string StatusPending = "Pending";
        public const string StatusDispatched = "Dispatched";
        public const string StatusProcessed = "Processed";
        public const string StatusError = "Error";
        public const string SettingsRowKey = "0-settings";

        public TableMessageEntity() { }

        public TableMessageEntity(string queueName)
        {
            PartitionKey = CreatePartitionKey(queueName);
            RowKey = SettingsRowKey;
            QueueName = queueName;
            Status = StatusSettings;
        }

        public TableMessageEntity(string queueName, int priority, string message)
        {
            PartitionKey = CreatePartitionKey(queueName);
            RowKey = $"{priority}-{DateTime.UtcNow.Ticks}-{Guid.NewGuid().ToString()}";
            QueueName = queueName;
            Message = message;
            Status = StatusPending;
        }


        /// <summary>
        /// The messageId
        /// </summary>
        public bool IsSettings => Status == StatusSettings;

        public string QueueName { get; set; }

        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The time when the message was dispatched.
        /// </summary>
        public DateTimeOffset? DispachedAt { get; set; }

        /// <summary>
        /// The application that processed the message
        /// </summary>
        public Guid? ProcessdBy { get; set; }
    }
}
