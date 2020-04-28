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
    public class MemoryQueueMethodBindingHandler : IMethodBindingHandler
    {
        public const string GenericInboundHandler = "generic";

        private static IDictionary<string, Func<string, Task>> s_Handlers = new Dictionary<string, Func<string, Task>>();

        public static Task HandleMessage(string queueName, string message)
        {
            if(!s_Handlers.TryGetValue(queueName, out Func<string, Task> handler))
            {
                throw new ArgumentException("No queue handler registered for queue:"+queueName);
            }
            return handler(message);
        }

        public MemoryQueueMethodBindingHandler(
            ILogger<MemoryQueueMethodBindingHandler> logger,
            IConfiguration configuration,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            MemoryQueueHandler queueHandler,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            Configuration = configuration;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
        }


        private ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private MemoryQueueHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// Invoked when a binding has been created. If there is a Queue transport
        /// configured - make sure that the queue exists 
        /// </summary>
        /// <param name="binding"></param>
        public void BindingCreated(IMethodBinding binding)
        {
            var queueTransport = binding.Transports.OfType<IQueueTransport>().Where(o => o.TransportType == QueueHandler.TransportType).FirstOrDefault();
            if (queueTransport == null)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"No queue transport({QueueHandler.TransportType}) configured for binding {binding.OperationId} - cannot configure queue");
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
            s_Handlers.Add(queueTransport.QueueName, msg => MessageHandler(msg, SolidRpcApplication.ShutdownToken));
        }

        private async Task MessageHandler(string msg, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Started processing message:" + msg);

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
                Logger.LogInformation("Completed processing message:" + msg);
            }
        }
    }
}
