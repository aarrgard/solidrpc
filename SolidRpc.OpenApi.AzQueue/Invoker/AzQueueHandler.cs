using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ITransportHandler), typeof(AzQueueHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(AzQueueHandler), typeof(AzQueueHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// Class responsible for doing the actual invocation.
    /// </summary>
    public class AzQueueHandler : QueueHandler<IAzQueueTransport>
    {
        public AzQueueHandler(
            ILogger<QueueHandler<IAzQueueTransport>> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, methodBinderStore, serializerFactory, solidRpcApplication)
        {
            CloudQueueStore = cloudQueueStore;
        }

        private ICloudQueueStore CloudQueueStore { get; }

        public override void Configure(IMethodBinding methodBinding, IAzQueueTransport transport)
        {
            base.Configure(methodBinding, transport);
        }

        protected override async Task InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, IAzQueueTransport transport, string message, InvocationOptions invocation, CancellationToken cancellationToken)
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
