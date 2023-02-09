using Azure.Messaging.ServiceBus;

namespace SolidRpc.OpenApi.AzSvcBus
{
    /// <summary>
    /// A store for the queue clients
    /// </summary>
    public interface IServiceBusClient
    {
        /// <summary>
        /// Returns the service bus receiver
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ServiceBusReceiver GetServiceBusReceiver(string connectionName, string queueName);

        /// <summary>
        /// Returns the queue client to access the queue at a specific connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ServiceBusSender GetServiceBusSender(string connectionName, string queueName);
    }
}
