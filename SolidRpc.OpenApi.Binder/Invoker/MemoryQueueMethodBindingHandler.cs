using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IMethodBindingHandler), typeof(MemoryQueueMethodBindingHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.Binder.Invoker
{ 
    /// <summary>
    /// Handles the queue transports
    /// </summary>
    public class MemoryQueueMethodBindingHandler : IMethodBindingHandler, IDisposable
    {
        public const string GenericInboundHandler = "generic";

        public MemoryQueueMethodBindingHandler(
            ILogger<MemoryQueueMethodBindingHandler> logger,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            MemoryQueueHandler queueHandler,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
            RegisteredQueues = new HashSet<string>();
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                MemoryQueueBus = scope.ServiceProvider.GetService<MemoryQueueBus>();
            }
            SolidRpcApplication.AddShutdownCallback(() => { Dispose(); return Task.CompletedTask; });
        }


        private ILogger Logger { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private QueueHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private HashSet<string> RegisteredQueues { get; }
        private MemoryQueueBus MemoryQueueBus { get; }

        /// <summary>
        /// Invoked when a binding has been created. If there is a Queue transport
        /// configured - make sure that the queue exists 
        /// </summary>
        /// <param name="binding"></param>
        public void BindingCreated(IMethodBinding binding)
        {
            if(MemoryQueueBus == null)
            {
                return;
            }
            var queueTransport = binding.Transports.OfType<IQueueTransport>().Where(o => o.TransportType == QueueHandler.TransportType).FirstOrDefault();
            if (queueTransport == null)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"No queue transport({QueueHandler.TransportType}) configured for binding {binding.OperationId} - will not setup queue");
                }
                return;
            }
            bool startReceiver = string.Equals(queueTransport.InboundHandler, GenericInboundHandler, StringComparison.InvariantCultureIgnoreCase);
            if (!startReceiver)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Inbound handler not {GenericInboundHandler}({queueTransport.InboundHandler}) {binding.OperationId} - will not startup generic receiver");
                }
                return;
            }
            RegisteredQueues.Add(queueTransport.QueueName);
            MemoryQueueBus.AddHandler(queueTransport.QueueName, msg => MessageHandler(msg, SolidRpcApplication.ShutdownToken));
        }

        private async Task MessageHandler(string msg, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Started processing message..");

                SolidRpcApplication.ShutdownToken.ThrowIfCancellationRequested();

                HttpRequest httpRequest;
                SerializerFactory.DeserializeFromString(msg, out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest);

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    await MethodInvoker.InvokeAsync(scope.ServiceProvider, QueueHandler, request, cancellationToken);
                }
            } 
            finally
            {
                Logger.LogInformation("...Completed processing message");
            }
        }

        public Task FlushQueuesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if(MemoryQueueBus == null)
            {
                return Task.CompletedTask;
            }
            return MemoryQueueBus.DispatchAllMessagesAsync();
        }

        public void Dispose()
        {
            var queueNames = new List<string>(RegisteredQueues);
            foreach (var queueName in queueNames)
            {
                MemoryQueueBus.RemoveHandler(queueName);
                RegisteredQueues.Remove(queueName);
            }
        }
    }
}
