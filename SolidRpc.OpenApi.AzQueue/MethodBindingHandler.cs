using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzQueue;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Services;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: SolidRpcService(typeof(IMethodBindingHandler), typeof(MethodBindingHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzQueue
{
    /// <summary>
    /// Handles the queue transports
    /// </summary>
    public class MethodBindingHandler : IMethodBindingHandler
    {
        public const string GenericInboundHandler = "generic";

        public MethodBindingHandler(
            ILogger<MethodBindingHandler> logger,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            AzQueueHandler queueHandler,
            IServiceProvider serviceProvider,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            CloudQueueStore = cloudQueueStore;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueHandler = queueHandler;
            ServiceProvider = serviceProvider;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
            FlushQueuesTasks = new List<Func<CancellationToken, Task>>();
        }

        private ILogger Logger { get; }
        public ICloudQueueStore CloudQueueStore { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private AzQueueHandler QueueHandler { get; }
        private IServiceProvider ServiceProvider { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        private BlobRequestOptions BlobRequestOptions => new BlobRequestOptions();
        private QueueRequestOptions QueueRequestOptions => new QueueRequestOptions();
        private OperationContext OperationContext => new OperationContext();
        private TableRequestOptions TableRequestOptions => new TableRequestOptions();

        private ICollection<Func<CancellationToken, Task>> FlushQueuesTasks { get; }

        /// <summary>
        /// Invoked when a binding has been created. If there is a Queue transport
        /// configured - make sure that the queue exists 
        /// </summary>
        /// <param name="binding"></param>
        public void BindingCreated(IMethodBinding binding)
        {

            //
            // start all the queues
            //
            binding.Transports.OfType<IQueueTransport>()
                .Where(o => string.Equals(o.TransportType, AzTableHandler.TransportType, StringComparison.InvariantCultureIgnoreCase))
                .ToList().ForEach(qt =>
                {
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.LogTrace($"Setting up AzTable binding for {binding.OperationId}.");
                    }
                    FlushQueuesTasks.Add((ct) => FlushTableTransport(binding, qt, ct));
                    SolidRpcApplication.AddStartupTask(SetupTable(binding, qt.ConnectionName, qt.QueueName));
                });

            binding.Transports.OfType<IQueueTransport>()
                .Where(o => string.Equals(o.TransportType, AzQueueHandler.TransportType, StringComparison.InvariantCultureIgnoreCase))
                .ToList().ForEach(qt =>
                {
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.LogTrace($"Setting up AzQueue binding for {binding.OperationId}.");
                    }
                    FlushQueuesTasks.Add((ct) => FlushQueueTransport(binding, qt, ct));

                    bool startReceiver = string.Equals(qt.InboundHandler, GenericInboundHandler, StringComparison.InvariantCultureIgnoreCase);
                    SolidRpcApplication.AddStartupTask(SetupQueue(binding, qt.ConnectionName, qt.QueueName, startReceiver));
                });
        }

        private async Task SetupTable(IMethodBinding binding, string connectionName, string queueName)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            await cloudTable.CreateIfNotExistsAsync(TableRequestOptions, OperationContext, SolidRpcApplication.ShutdownToken);
        }

        /// <summary>
        /// Checks that the supplied 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private async Task SetupQueue(IMethodBinding binding, string connectionName, string queueName, bool startReceiver)
        {
            var cloudQueue = CloudQueueStore.GetCloudQueue(connectionName, queueName);

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Ensuring that the queue {cloudQueue.Name} exists.");
            }
            await cloudQueue.CreateIfNotExistsAsync(QueueRequestOptions, OperationContext, SolidRpcApplication.ShutdownToken);
            await CloudQueueStore.GetCloudBlobContainer(connectionName).CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, BlobRequestOptions, OperationContext, SolidRpcApplication.ShutdownToken);

            if (startReceiver)
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Starting generic inbound handler for operation {binding.OperationId}.");
                }
                StartMessagePump(connectionName, cloudQueue);
            }
        }

        private async Task StartMessagePump(string connectionName, CloudQueue cloudQueue)
        {
            int emptyQueueCount = 0;
            var cancellationToken = SolidRpcApplication.ShutdownToken;
            while (!cancellationToken.IsCancellationRequested)
            {
                if(Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Fetching message from queue:{cloudQueue.Name}");
                }
                try
                {
                    var msg = await cloudQueue.GetMessageAsync(
                        new TimeSpan(0, 1, 0),
                        QueueRequestOptions,
                        OperationContext,
                        cancellationToken
                        );

                    if (msg != null)
                    {
                        emptyQueueCount = 0;
                        if (await ProcessMessage(connectionName, msg, cancellationToken))
                        {
                            await CloudQueueStore.DeleteLargeMessageAsync(connectionName, msg.AsString, cancellationToken);
                            await cloudQueue.DeleteMessageAsync(msg.Id, msg.PopReceipt, QueueRequestOptions, OperationContext, cancellationToken);
                        }
                        continue;
                    }
                }
                catch (StorageException se)
                {
                    if(se.RequestInformation.ErrorCode == "QueueNotFound")
                    {
                        // continue - the the queue might become available.
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error fetching message");
                }

                var delay = Math.Pow(2, emptyQueueCount);
                delay = Math.Min(delay, 60);
                await Task.Delay((int)(delay * 1000));
                emptyQueueCount++;
            }
        }

        private async Task<bool> ProcessMessage(string connectionName, CloudQueueMessage msg, CancellationToken cancellationToken)
        {
            try
            {
                if(Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Started processing message:" + msg.Id);
                }

                var strMsg = await CloudQueueStore.RetreiveLargeMessageAsync(connectionName, msg.AsString, cancellationToken);

                HttpRequest httpRequest;
                SerializerFactory.DeserializeFromString(strMsg, out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest);

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var resp = await MethodInvoker.InvokeAsync(scope.ServiceProvider, QueueHandler, request, cancellationToken);
                    if(resp.StatusCode >= 200 && resp.StatusCode < 300)
                    {
                        // ok
                    }
                    else
                    {
                        throw new Exception("Invocation not successful:" + resp.StatusCode);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Error processing message {msg.Id}");
                return false;
            }
            finally
            {
                Logger.LogInformation("Completed processing message:" + msg.Id);
            }
        }

        public Task FlushQueuesAsync(CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(FlushQueuesTasks.Select(o => o(cancellationToken)));
        }

        private async Task FlushTableTransport(IMethodBinding binding, IQueueTransport qt, CancellationToken cancellationToken)
        {
            await ServiceProvider.GetRequiredService<IAzTableQueue>().DoScheduledScanAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                // find all the rows for the binding
                var ct = CloudQueueStore.GetCloudTable(qt.ConnectionName);
                var messages = await Services.AzTableQueue.GetMessagesAsync(ct, qt.QueueName, new[] { TableMessageEntity.StatusPending, TableMessageEntity.StatusDispatched }, 1, cancellationToken);
                if(messages.Count() == 0)
                {
                    return;
                }
                await Task.Delay(100);
            }
        }

        private async Task FlushQueueTransport(IMethodBinding binding, IQueueTransport qt, CancellationToken cancellationToken)
        {
            var cq = CloudQueueStore.GetCloudQueue(qt.ConnectionName, qt.QueueName);
            try
            {
                while(!cancellationToken.IsCancellationRequested)
                {
                    await cq.FetchAttributesAsync();
                    if (cq.ApproximateMessageCount == 0)
                    {
                        if(Logger.IsEnabled(LogLevel.Trace))
                        {
                            Logger.LogTrace($"AzQueue {qt.QueueName} is empty");
                        }
                        return;
                    }
                    await Task.Delay(500);
                }
            }
            catch (StorageException se)
            {
                if(se.RequestInformation.ErrorCode == "QueueNotFound")
                {
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.LogTrace($"AzQueue {qt.QueueName} not found - empty");
                    }
                    return;
                }
                throw;
            }
        }
    }
}
