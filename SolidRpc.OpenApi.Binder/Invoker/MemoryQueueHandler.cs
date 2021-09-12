using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ITransportHandler), typeof(MemoryQueueHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(MemoryQueueHandler), typeof(MemoryQueueHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    public class MemoryQueueHandler : QueueHandler<IMemoryQueueTransport>
    {
        public MemoryQueueHandler(
            ILogger<QueueHandler<IMemoryQueueTransport>> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory, 
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
        }

        private MemoryQueueBus MemoryQueueBus => ServiceProvider.GetRequiredService<MemoryQueueBus>();

        public override void Configure(IMethodBinding methodBinding, IMemoryQueueTransport transport)
        {
            base.Configure(methodBinding, transport);
        }

        protected override Task InvokeAsync(IMethodBinding methodBinding, IMemoryQueueTransport transport, string message, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("MemoryQueue dispatching message...");
                return MemoryQueueBus.HandleMessage(transport.QueueName, message, invocationOptions);
            }
            finally
            {
                Logger.LogInformation("...MemoryQueue dispatched message");
            }
        }
    }
}
