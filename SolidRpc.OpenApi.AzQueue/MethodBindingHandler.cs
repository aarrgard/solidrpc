﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
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
using SolidRpc.OpenApi.Binder.Http;
using System;
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
        public const string AzQueueTableType = "AzTable";
        public const string AzQueueQueueType = "AzQueue";
        public const string GenericInboundHandler = "generic";

        public MethodBindingHandler(
            ILogger<MethodBindingHandler> logger,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication,
            ISerializerFactory serializerFactory,
            IMethodInvoker methodInvoker,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            CloudQueueStore = cloudQueueStore;
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            MethodInvoker = methodInvoker;
            ServiceScopeFactory = serviceScopeFactory;
        }

        private ILogger Logger { get; }
        public ICloudQueueStore CloudQueueStore { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        private QueueRequestOptions QueueRequestOptions => new QueueRequestOptions();
        private OperationContext OperationContext => new OperationContext();
        private TableRequestOptions TableRequestOptions => new TableRequestOptions();

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
            if (string.Equals(queueTransport.QueueType, AzQueueTableType, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Queue type({queueTransport.QueueType}) is a {AzQueueTableType} for binding {binding.OperationId} - setting up table.");
                }
                SolidRpcApplication.AddStartupTask(SetupTable(binding, queueTransport.ConnectionName, queueTransport.QueueName));
                return;
            }
            if (!string.Equals(queueTransport.QueueType, AzQueueQueueType, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Queue type({queueTransport.QueueType}) is not a {AzQueueQueueType} for binding {binding.OperationId} - will not configure az queue");
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
            }
            SolidRpcApplication.AddStartupTask(SetupQueue(binding, queueTransport.ConnectionName, queueTransport.QueueName, startReceiver));
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

            if(Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Ensuring that the queue {cloudQueue.Name} exists.");
            }
            await cloudQueue.CreateIfNotExistsAsync(QueueRequestOptions, OperationContext, SolidRpcApplication.ShutdownToken);

            if (startReceiver)
            {
                StartMessagePump(cloudQueue);
            }
        }

        private async Task StartMessagePump(CloudQueue cloudQueue)
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
                        if (await ProcessMessage(msg, cancellationToken))
                        {
                            await cloudQueue.DeleteAsync(QueueRequestOptions, OperationContext, cancellationToken);
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

        private async Task<bool> ProcessMessage(CloudQueueMessage msg, CancellationToken cancellationToken)
        {
            try
            {
                if(Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Started processing message:" + msg.Id);
                }

                HttpRequest httpRequest;
                SerializerFactory.DeserializeFromString(msg.AsString, out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest);

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var resp = await MethodInvoker.InvokeAsync(scope.ServiceProvider, request, cancellationToken);
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
    }
}
