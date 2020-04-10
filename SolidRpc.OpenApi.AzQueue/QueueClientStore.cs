using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzQueue;
using System;
using System.Collections.Concurrent;

[assembly: SolidRpcService(typeof(ICloudQueueStore), typeof(CloudQueueStore), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue
{
    public class CloudQueueStore : ICloudQueueStore
    {
        public CloudQueueStore(ILogger<CloudQueueStore> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            CloudQueues = new ConcurrentDictionary<string, ConcurrentDictionary<string, CloudQueue>>();
            CloudTables = new ConcurrentDictionary<string, ConcurrentDictionary<string, CloudTable>>();
        }
        ILogger Logger { get; }
        IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudQueue>> CloudQueues { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudTable>> CloudTables { get; }
        public CloudQueue GetCloudQueue(string connectionName, string queueName)
        {
            var dict = CloudQueues.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudQueue>());
            return dict.GetOrAdd(queueName, _ => CreateCloudQueue(connectionName, queueName));
        }

        public CloudTable GetCloudTable(string connectionName)
        {
            var tableName = "SolidRpcQueue";
            var dict = CloudTables.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudTable>());
            return dict.GetOrAdd(tableName, _ => CreateCloudTable(connectionName, tableName));
        }

        private CloudTable CreateCloudTable(string connectionName, string tableName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:" + connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference(tableName);
        }

        private CloudQueue CreateCloudQueue(string connectionName, string queueName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:"+connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            var queueClient = storageAccount.CreateCloudQueueClient();

            return queueClient.GetQueueReference(queueName);
        }
    }
}
