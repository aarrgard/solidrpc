using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IHandler), typeof(MemoryQueueHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(MemoryQueueHandler), typeof(MemoryQueueHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
   public class MemoryQueueHandler : QueueHandler
    {
        /// <summary>
        /// Returns the "MemoryQueue" transport type
        /// </summary>
        public static new string TransportType => GetTransportType(typeof(MemoryQueueHandler));

        public MemoryQueueHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory, 
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
        }

        protected override Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("MemoryQueue dispatching message...");
                return MemoryQueueMethodBindingHandler.HandleMessage(transport.QueueName, message);
            }
            finally
            {
                Logger.LogInformation("...MemoryQueue dispatched message");
            }
        }
    }
}
