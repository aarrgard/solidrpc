using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.AzSvcBus.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ITransportHandler), typeof(SvcBusHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzSvcBus.Invoker
{
    public class SvcBusHandler : QueueHandler<ISvcBusTransport>
    {
        public SvcBusHandler(ILogger<QueueHandler<ISvcBusTransport>> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            IServiceBusClient serviceBusClient,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, methodBinderStore, serializerFactory, solidRpcApplication)
        {
            ServiceBusClient = serviceBusClient;
        }

        private IServiceBusClient ServiceBusClient { get; }

        protected override Task InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, ISvcBusTransport queueTransport, string message, CancellationToken cancellationToken)
        {
            // dispatch message
            var msg = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));
            msg.ContentType = "application/json";
            var qc = ServiceBusClient.GetServiceBusSender(queueTransport.ConnectionName, queueTransport.QueueName);
            return qc.SendMessageAsync(msg, cancellationToken);
        }
    }
}
