using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace SolidRpc.OpenApi.AzQueue
{
    public class TableMessageEntry : TableEntity
    {
        public TableMessageEntry(string queueName, string message)
        {
            PartitionKey = queueName;
            RowKey = Guid.NewGuid().ToString();
            ETag = "*";
        }
    }
}
