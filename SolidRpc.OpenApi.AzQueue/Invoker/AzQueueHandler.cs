using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IHandler), typeof(AzQueueHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(AzQueueHandler), typeof(AzQueueHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// Class responsible for doing the actual invocation.
    /// </summary>
    public class AzQueueHandler : QueueHandler
    {
        /// <summary>
        /// The transport type
        /// </summary>
        public static readonly new string TransportType = GetTransportType(typeof(AzQueueHandler));

        public AzQueueHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
            CloudQueueStore = cloudQueueStore;
        }

        private ICloudQueueStore CloudQueueStore { get; }

        protected override async Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, InvocationOptions invocation, CancellationToken cancellationToken)
        {
            message = await CloudQueueStore.StoreLargeMessageAsync(transport.ConnectionName, message);
            var msg = new CloudQueueMessage(message);
            var qc = CloudQueueStore.GetCloudQueue(transport.ConnectionName, transport.QueueName);
            await qc.AddMessageAsync(msg);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Sent message {msg.Id}");
            }
        }
    }
}
