using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzSvcBus.Invoker
{
    public class SvcBusHandler : QueueHandler
    {
        public SvcBusHandler(ILogger<QueueHandler> logger,
            IServiceProvider serviceProvider,
            ISerializerFactory serializerFactory,
            IQueueClientStore queueClientStore,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
            QueueClientStore = queueClientStore;
        }

        private IQueueClientStore QueueClientStore { get; }

        protected override Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport queueTransport, string message, CancellationToken cancellationToken)
        {
            // dispatch message
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            msg.ContentType = "application/json";
            var qc = QueueClientStore.GetQueueClient(queueTransport.ConnectionName, queueTransport.QueueName);
            return qc.SendAsync(msg);
        }
    }
}
