using Microsoft.Azure.ServiceBus;
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
            IServiceProvider serviceProvider,
            ISerializerFactory serializerFactory,
            IQueueClientStore queueClientStore,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
            QueueClientStore = queueClientStore;
        }

        private IQueueClientStore QueueClientStore { get; }

        protected override Task InvokeAsync(IMethodBinding methodBinding, ISvcBusTransport queueTransport, string message, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            // dispatch message
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            msg.ContentType = "application/json";
            var qc = QueueClientStore.GetQueueClient(queueTransport.ConnectionName, queueTransport.QueueName);
            return qc.SendAsync(msg);
        }
    }
}
