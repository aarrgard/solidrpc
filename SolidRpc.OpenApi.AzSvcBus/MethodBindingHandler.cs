using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
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
using SolidRpc.OpenApi.AzSvcBus;
using SolidRpc.OpenApi.Binder.Http;
using System;
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
        public const string SvcBusQueueType = "AzSvcBus";
        public const string GenericInboundHandler = "generic";

        public MethodBindingHandler(
            ILogger<MethodBindingHandler> logger,
            IConfiguration configuration,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            Configuration = configuration;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
        }

        private ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// Invoked when a binding has been created. If there is a Queue transport
        /// configured - make sure that the queue exists 
        /// </summary>
        /// <param name="binding"></param>
        public void BindingCreated(IMethodBinding binding)
        {
            var queueTransport = binding.Transports.OfType<IQueueTransport>().FirstOrDefault();
            if (queueTransport == null)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"No queue transport configured for binding {binding.OperationId} - cannot configure queue");
                }
                return;
            }
            if (!string.Equals(queueTransport.TransportType, SvcBusQueueType, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Queue type({queueTransport.TransportType}) is not a {SvcBusQueueType} for binding {binding.OperationId} - will not configure svcbus queue");
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
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Cannot find connection string for connection name:" + connectionName);
            }

            //
            // make sure that the queue exists
            //
            var mgmt = new ManagementClient(connectionString);
            try
            {
                var queue = await mgmt.GetQueueAsync(queueName, cancellationToken);
            }
            catch (Exception e)
            {
                var queueDescription = new QueueDescription(queueName);
                queueDescription.AutoDeleteOnIdle = new TimeSpan(1, 0, 0, 0);
                var queue = await mgmt.CreateQueueAsync(queueName, cancellationToken);
            }

            if(startReceiver)
            {
                var gc = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
                gc.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ExceptionHandler)
                {
                    MaxConcurrentCalls = 100,
                    AutoComplete = true
                });
            }
        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Logger.LogError(arg.Exception, "Error processing message:");
            return Task.CompletedTask;
        }

        private async Task MessageHandler(Message msg, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Started processing message:" + msg.MessageId);

                HttpRequest httpRequest;
                var ms = new MemoryStream(msg.Body);
                SerializerFactory.DeserializeFromStream(ms, out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest);

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    await MethodInvoker.InvokeAsync(scope.ServiceProvider, request, cancellationToken);
                }
            } 
            finally
            {
                Logger.LogInformation("Completed processing message:" + msg.MessageId);
            }
        }
    }
}
