using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzSvcBus;
using SolidRpc.OpenApi.AzSvcBus.Invoker;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IMethodBindingHandler), typeof(MethodBindingHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzSvcBus
{
    /// <summary>
    /// Handles the queue transports
    /// </summary>
    public class MethodBindingHandler : IMethodBindingHandler
    {
        public const string GenericInboundHandler = "generic";

        public MethodBindingHandler(
            ILogger<MethodBindingHandler> logger,
            IConfiguration configuration,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            SvcBusHandler queueHandler,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory,
            IServiceBusClient serviceBusClient)
        {
            Logger = logger;
            Configuration = configuration;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
            ServiceBusClient = serviceBusClient;
        }

        private ILogger Logger { get; }
        private IConfiguration Configuration { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private SvcBusHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IServiceBusClient ServiceBusClient { get; }

        /// <summary>
        /// Invoked when a binding has been created. If there is a Queue transport
        /// configured - make sure that the queue exists 
        /// </summary>
        /// <param name="binding"></param>
        public void BindingCreated(IMethodBinding binding)
        {
            var queueTransport = binding.Transports.OfType<ISvcBusTransport>().FirstOrDefault();
            if (queueTransport == null)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"No queue transport({SvcBusHandler.TransportType}) configured for binding {binding.OperationId} - cannot configure queue");
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
            SolidRpcApplication.AddStartupTask(SetupQueueReceiver(binding, queueTransport.ConnectionName, queueTransport.QueueName, startReceiver, SolidRpcApplication.ShutdownToken));
        }

        /// <summary>
        /// Checks that the supplied 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private async Task SetupQueueReceiver(IMethodBinding binding, string connectionName, string queueName, bool startReceiver, CancellationToken cancellationToken)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Cannot find connection string for connection name:" + connectionName);
            }

            //
            // make sure that the queue exists
            //
            var mgmt = new ServiceBusAdministrationClient(connectionString);
            try
            {
                var queue = await mgmt.GetQueueAsync(queueName, cancellationToken);
            }
            catch (Exception)
            {
                var queueDescription = new CreateQueueOptions(queueName);
                //queueDescription.AutoDeleteOnIdle = new TimeSpan(1, 0, 0, 0);
                var queue = await mgmt.CreateQueueAsync(queueDescription, cancellationToken);
            }

            if (startReceiver)
            {
                StartReceivingMessagesAsync(connectionName, queueName);
            }
        }

        private async Task StartReceivingMessagesAsync(string connectionName, string queueName)
        {
            await Task.Yield();
            var receiver = ServiceBusClient.GetServiceBusReceiver(connectionName, queueName);
            while(!SolidRpcApplication.ShutdownToken.IsCancellationRequested)
            {
                var msg = await receiver.ReceiveMessageAsync(null, SolidRpcApplication.ShutdownToken);
                try
                {
                    await MessageHandler(msg, SolidRpcApplication.ShutdownToken);
                    await receiver.CompleteMessageAsync(msg, SolidRpcApplication.ShutdownToken);
                } catch (Exception ex) 
                {
                    await receiver.DeadLetterMessageAsync(msg, new Dictionary<string ,object>() { }, SolidRpcApplication.ShutdownToken);
                }
            }
        }

        private async Task MessageHandler(ServiceBusReceivedMessage msg, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Started processing message:" + msg.MessageId);

                HttpRequest httpRequest;
                SerializerFactory.DeserializeFromStream(msg.Body.ToStream(), out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest, p => p);

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    await MethodInvoker.InvokeAsync(scope.ServiceProvider, QueueHandler, request, cancellationToken);
                }
            }
            finally
            {
                Logger.LogInformation("Completed processing message:" + msg.MessageId);
            }
        }

        public Task FlushQueuesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
