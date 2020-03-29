using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzQueue;
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
        }
        ILogger Logger { get; }
        IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudQueue>> CloudQueues { get; }
        public CloudQueue GetQueueClient(string connectionName, string queueName)
        {
            var dict = CloudQueues.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudQueue>());
            return dict.GetOrAdd(queueName, _ => CreateQueueClient(connectionName, queueName));
        }

        private CloudQueue CreateQueueClient(string connectionName, string queueName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:"+connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            return queueClient.GetQueueReference(queueName);
        }
    }
}
