using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzSvcBus;
using System;
using System.Collections.Concurrent;

[assembly: SolidRpcService(typeof(IServiceBusClient), typeof(QueueClientStore), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzSvcBus
{
    public class QueueClientStore : IServiceBusClient
    {
        public QueueClientStore(ILogger<QueueClientStore> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            ServiceBusClients = new ConcurrentDictionary<string, ServiceBusClient>();
            ServiceBusSenders = new ConcurrentDictionary<string, ConcurrentDictionary<string, ServiceBusSender>> ();
        }
        ILogger Logger { get; }
        IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ServiceBusClient> ServiceBusClients { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ServiceBusSender>> ServiceBusSenders { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ServiceBusReceiver>> ServiceBusReceivers { get; }

        private ServiceBusClient GetServiceBusClient(string connectionName)
        {
            return ServiceBusClients.GetOrAdd(connectionName, _ => {
                var connectionString = Configuration[connectionName];
                return new ServiceBusClient(connectionString);
            });
        }

        public ServiceBusSender GetServiceBusSender(string connectionName, string queueName)
        {
            return ServiceBusSenders
                .GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, ServiceBusSender>())
                .GetOrAdd(queueName, _ => GetServiceBusClient(connectionName).CreateSender(queueName));
        }

        public ServiceBusReceiver GetServiceBusReceiver(string connectionName, string queueName)
        {
            return ServiceBusReceivers
                .GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, ServiceBusReceiver>())
                .GetOrAdd(queueName, _ => GetServiceBusClient(connectionName).CreateReceiver(queueName, new ServiceBusReceiverOptions()
                {
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                }));
        }
    }
}
