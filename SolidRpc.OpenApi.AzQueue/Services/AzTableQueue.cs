using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SolidRpc.OpenApi.AzQueue.Services
{
    public class AzTableQueue : IAzTableQueue
    {
        private static TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private static OperationContext OperationContext => new OperationContext();

        public AzTableQueue(
            ILogger<AzTableQueue> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            IServiceProvider serviceProvider,
            AzTableHandler queueHandler,
            IMethodInvoker methodInvoker,
            ISolidRpcApplication solidRpcApplication)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            SerializerFactory = serializerFactory;
            CloudQueueStore = cloudQueueStore;
            ServiceProvider = serviceProvider;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            SolidRpcApplication = solidRpcApplication;
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }
        private IServiceProvider ServiceProvider { get; }
        private AzTableHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }
        
        private IInvoker<IAzTableQueue> Invoker => ServiceProvider.GetRequiredService<IInvoker<IAzTableQueue>>();

        private IEnumerable<IQueueTransport> GetTableTransports()
        {
            // find all the table queues
            return MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .SelectMany(o => o.Transports)
                .OfType<IQueueTransport>()
                .Where(o => o.TransportType == "AzTable")
                .ToList();
        }

        public Task DoScheduledScanAsync(CancellationToken cancellationToken = default)
        {
            // find all the table queues
            var tableTranports = GetTableTransports()
                .Select(o => new { o.ConnectionName, o.QueueName })
                .Distinct()
                .ToList();

            var tasks = tableTranports.Select(o => QueueHandler.DispatchMessageAsync(o.ConnectionName, o.QueueName, true, cancellationToken));
            return Task.WhenAll(tasks);
        }

        public Task DispatchMessageAsync(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            return QueueHandler.DispatchMessageAsync(connectionName, queueName, false, cancellationToken);
        }

        public async Task ProcessMessageAsync(string connectionName, string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var result = await cloudTable.ExecuteAsync(TableOperation.Retrieve<TableMessageEntity>(partitionKey, rowKey), TableRequestOptions, OperationContext, cancellationToken);
            var row = (TableMessageEntity)result?.Result;
            if(row == null)
            {
                Logger.LogError($"Failed to retreive message:{connectionName}:{partitionKey}:{rowKey}");
                return;
            }

            var message = await CloudQueueStore.RetreiveLargeMessageAsync(connectionName, row.Message, cancellationToken);

            HttpRequest httpRequest;
            SerializerFactory.DeserializeFromString(message, out httpRequest);

            var request = new SolidHttpRequest();
            await request.CopyFromAsync(httpRequest);

            var resp = await MethodInvoker.InvokeAsync(ServiceProvider, QueueHandler, request);
            if(resp.StatusCode >= 200 && resp.StatusCode < 300)
            {
                // row processed ok - delete it
                await CloudQueueStore.DeleteLargeMessageAsync(connectionName, row.Message, cancellationToken);
                await cloudTable.ExecuteAsync(TableOperation.Delete(row), TableRequestOptions, OperationContext, cancellationToken);
            }
            else
            {
                row.Status = TableMessageEntity.StatusError;
                row.ProcessdBy = SolidRpcApplication.HostId;
                await cloudTable.ExecuteAsync(TableOperation.Replace(row), TableRequestOptions, OperationContext, cancellationToken);
            }

            await QueueHandler.DispatchMessageAsync(connectionName, row.QueueName, false, cancellationToken);
        }

        public async Task<IEnumerable<AzTableQueueSettings>> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            var tasks = GetTableTransports().Select(o => GetSettingsAsync(o.ConnectionName, o.QueueName, cancellationToken));
            return await Task.WhenAll(tasks);
        }

        private async Task<AzTableQueueSettings> GetSettingsAsync(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            try
            {
                var result = await cloudTable.ExecuteAsync(TableOperation.Retrieve<TableMessageEntity>(TableMessageEntity.CreatePartitionKey(queueName), TableMessageEntity.SettingsRowKey), TableRequestOptions, OperationContext, cancellationToken);
                var settings = QueueHandler.DeserializeSettings(((TableMessageEntity)result?.Result)?.Message);
                if(settings == null)
                {
                    settings = new AzTableQueueSettings() { QueueName = queueName, MaxConcurrentCalls = 1 };
                    await UpdateSettings(settings);
                }
                return settings;
            }
            catch(StorageException se)
            {
                throw;
            }
        }

        public async Task<AzTableQueueSettings> UpdateSettings(AzTableQueueSettings settings, CancellationToken cancellationToken = default)
        {
            var transport = GetTableTransports().FirstOrDefault(o => o.QueueName == settings.QueueName);
            if (transport == null) throw new ArgumentException("Cannot find transport for queue name.");
            var cloudTable = CloudQueueStore.GetCloudTable(transport.ConnectionName);
            var settingRow = new TableMessageEntity(settings.QueueName);
            settingRow.Message = QueueHandler.SerializeSettings(settings);
            try
            {
                await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(settingRow), TableRequestOptions, OperationContext, cancellationToken);
                return settings;
            }
            catch(StorageException se)
            {
                throw;
            }
        }

        public async Task SendTestMessageAync(Stream payload, int messageCount = 1, bool raiseException = false, int messagePriority = 5, CancellationToken cancellationToken = default)
        {
            var ms = new MemoryStream();
            await payload.CopyToAsync(ms);

            var tasks = new List<Task>();
            for(int i = 0; i < messageCount; i++)
            {
                tasks.Add(Invoker.InvokeAsync(o => o.ProcessTestMessage(new MemoryStream(ms.ToArray()), raiseException, cancellationToken), InvocationOptions.AzTable.SetPriority(messagePriority)));
            }
            await Task.WhenAll(tasks);
        }

        public Task ProcessTestMessage(Stream payload, bool raiseException, CancellationToken cancellationToken = default)
        {
            while (payload.ReadByte() > 0);
            if (raiseException) throw new Exception("Exception in test method.");
            return Task.CompletedTask;
        }

        public Task FlagErrorMessagesAsPending(CancellationToken cancellationToken = default)
        {
            // find all the table queues
            var tasks = GetTableTransports()
                .Select(o => new { o.ConnectionName, o.QueueName })
                .Distinct()
                .Select(o => QueueHandler.FlagErrorMessagesAsPending(o.ConnectionName, o.QueueName, cancellationToken));

            return Task.WhenAll(tasks);
        }
    }
}
