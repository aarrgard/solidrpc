using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzSvcBus;
using System;
using System.Collections.Concurrent;

[assembly: SolidRpcService(typeof(IQueueClientStore), typeof(QueueClientStore), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzSvcBus
{
    public class QueueClientStore : IQueueClientStore
    {
        public QueueClientStore(ILogger<QueueClientStore> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            QueueClients = new ConcurrentDictionary<string, ConcurrentDictionary<string, QueueClient>>();
        }
        ILogger Logger { get; }
        IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, QueueClient>> QueueClients { get; }
        public QueueClient GetQueueClient(string connectionName, string queueName)
        {
            var dict = QueueClients.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, QueueClient>());
            return dict.GetOrAdd(queueName, _ => CreateQueueClient(connectionName, queueName));
        }

        private QueueClient CreateQueueClient(string connectionName, string queueName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:"+connectionName);
            }
            return new QueueClient(connectionString, queueName);
        }
    }
}
