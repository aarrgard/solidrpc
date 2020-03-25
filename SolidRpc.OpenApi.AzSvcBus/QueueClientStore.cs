using Microsoft.Azure.ServiceBus;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzSvcBus;
using System.Collections.Concurrent;

[assembly: SolidRpcAbstractionProvider(typeof(IQueueClientStore), typeof(QueueClientStore), SolidRpcAbstractionProviderLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzSvcBus
{
    public class QueueClientStore : IQueueClientStore
    {
        public QueueClientStore()
        {
            QueueClients = new ConcurrentDictionary<string, ConcurrentDictionary<string, QueueClient>>();
        }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, QueueClient>> QueueClients { get; }
        public QueueClient GetQueueClient(string connectionString, string queueName)
        {

            var dict = QueueClients.GetOrAdd(connectionString, _ => new ConcurrentDictionary<string, QueueClient>());
            return dict.GetOrAdd(queueName, _ => new QueueClient(connectionString, queueName));
        }
    }
}
