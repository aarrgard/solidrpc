using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzQueue.Services
{
    public class AzTableQueue : IAzTableQueue
    {
        private static TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private static OperationContext OperationContext => new OperationContext();

        public AzTableQueue(
            ILogger<AzTableQueue> logger,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            IServiceProvider serviceProvider,
            AzTableHandler queueHandler,
            IMethodInvoker methodInvoker,
            IInvoker<IAzTableQueue> invoker,
            ISolidRpcApplication solidRpcApplication)
        {
            Logger = logger;
            SerializerFactory = serializerFactory;
            CloudQueueStore = cloudQueueStore;
            ServiceProvider = serviceProvider;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            Invoker = invoker;
            SolidRpcApplication = solidRpcApplication;
        }

        private ILogger Logger { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }
        private IServiceProvider ServiceProvider { get; }
        private AzTableHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        private IInvoker<IAzTableQueue> Invoker { get; }
        private ISolidRpcApplication SolidRpcApplication { get; }

        public Task DoScheduledScanAsync(CancellationToken cancellationToken = default)
        {
            // find all the table queues
            var tableTranports = QueueHandler.GetTableTransports()
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
            if (message == null)
            {
                Logger.LogError($"Failed to retreive large message:{connectionName}:{partitionKey}:{rowKey}");
                await cloudTable.ExecuteAsync(TableOperation.Delete(row), TableRequestOptions, OperationContext, cancellationToken);
                return;
            }

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
            var tasks = QueueHandler.GetTableTransports().Select(o => GetSettingsAsync(o.ConnectionName, o.QueueName, cancellationToken));
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

        public Task<AzTableQueueSettings> UpdateSettings(AzTableQueueSettings settings, CancellationToken cancellationToken = default)
        {
            return QueueHandler.UpdateSettings(settings, cancellationToken);
        }

        public async Task SendTestMessageAsync(Stream payload, int messageCount = 1, bool raiseException = false, int messagePriority = 5, CancellationToken cancellationToken = default)
        {
            var ms = new MemoryStream();
            await payload.CopyToAsync(ms);

            var tasks = new List<Task>();
            for(int i = 0; i < messageCount; i++)
            {
                tasks.Add(Invoker.InvokeAsync(o => o.ProcessTestMessage(new MemoryStream(ms.ToArray()), raiseException, cancellationToken), opt => opt.SetTransport(AzTableHandler.TransportType).SetPriority(messagePriority)));
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
            var tasks = QueueHandler.GetTableTransports()
                .Select(o => new { o.ConnectionName, o.QueueName })
                .Distinct()
                .Select(o => QueueHandler.FlagErrorMessagesAsPending(o.ConnectionName, o.QueueName, cancellationToken));

            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<AzTableMessage>> ListMessagesAsync(CancellationToken cancellationToken = default)
        {
            var tasks = QueueHandler.GetTableTransports()
                .Select(o => new { o.ConnectionName, o.QueueName })
                .Distinct()
                .Select(o => ListMessagesAsync(o.ConnectionName, o.QueueName, cancellationToken));
            return (await Task.WhenAll(tasks)).SelectMany(o => o);
        }

        private async Task<IEnumerable<AzTableMessage>> ListMessagesAsync(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var messages = await AzTableHandler.GetMessagesAsync(cloudTable, queueName, TableMessageEntity.AllStatuses, int.MaxValue, cancellationToken);

            var azMessages = messages.Select(o => new AzTableMessage()
            {
                ConnectionName = connectionName,
                PartitionKey = o.PartitionKey,
                RowKey = o.RowKey,
                Status = o.Status,
                Message = o.Message
            }).ToArray();

            await Task.WhenAll(azMessages.Select(o => SetLargeMessage(connectionName, o, cancellationToken)));

            return azMessages.Where(o => o.Message != null).ToList();
        }

        private async Task SetLargeMessage(string connectionName, AzTableMessage msg, CancellationToken cancellationToken)
        {
            msg.Message = await CloudQueueStore.RetreiveLargeMessageAsync(connectionName, msg.Message, cancellationToken);

        }

        public async Task UpdateMessageAsync(AzTableMessage msg, CancellationToken cancellationToken = default)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(msg.ConnectionName);
            var res = await cloudTable.ExecuteAsync(TableOperation.Retrieve<TableMessageEntity>(msg.PartitionKey, msg.RowKey), TableRequestOptions, OperationContext, cancellationToken);
            var row = (TableMessageEntity)res.Result;
            if (row == null) throw new Exception("Cannot find row.");
            row.Message = msg.Message;
            row.Status = msg.Status;

            var oldMsg = row.Message;
            row.Message = await CloudQueueStore.StoreLargeMessageAsync(msg.ConnectionName, row.Message, cancellationToken);
            await cloudTable.ExecuteAsync(TableOperation.Merge(row), TableRequestOptions, OperationContext, cancellationToken);
            await CloudQueueStore.DeleteLargeMessageAsync(msg.ConnectionName, oldMsg, cancellationToken);
        }
    }
}
