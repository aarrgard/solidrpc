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
using SolidRpc.OpenApi.AzQueue.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SolidRpc.OpenApi.AzQueue.Services
{
    public class AzTableQueue : IAzTableQueue
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> Semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

        public AzTableQueue(
            ILogger<AzTableQueue> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            IQueueInvoker<IAzTableQueue> queueInvoker,
            IServiceProvider serviceProvider,
            IMethodInvoker methodInvoker,
            ISolidRpcApplication solidRpcApplication)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            SerializerFactory = serializerFactory;
            CloudQueueStore = cloudQueueStore;
            QueueInvoker = queueInvoker;
            ServiceProvider = serviceProvider;
            MethodInvoker = methodInvoker;
            SolidRpcApplication = solidRpcApplication;
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }
        private IQueueInvoker<IAzTableQueue> QueueInvoker { get; }
        private IServiceProvider ServiceProvider { get; }
        private IMethodInvoker MethodInvoker { get; }
        public ISolidRpcApplication SolidRpcApplication { get; }

        private TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private OperationContext OperationContext => new OperationContext();

        private IEnumerable<IQueueTransport> GetTableTransports()
        {
            // find all the table queues
            return MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .SelectMany(o => o.Transports)
                .OfType<IQueueTransport>()
                .Where(o => o.QueueType == "AzTable")
                .ToList();
        }

        public Task DispatchMessageAsync(CancellationToken cancellationToken = default)
        {
            // find all the table queues
            var tableTranports = GetTableTransports()
                .Select(o => new { o.ConnectionName, o.QueueName })
                .Distinct()
                .ToList();

            var tasks = tableTranports.Select(o => DispatchMessage(o.ConnectionName, o.QueueName, true, cancellationToken));
            return Task.WhenAll(tasks);
        }

        private async Task DispatchMessage(string connectionName, string queueName, bool scheduledScan, CancellationToken cancellationToken)
        {
            var semaphore = Semaphores.GetOrAdd($"{connectionName}:{queueName}", _ => new SemaphoreSlim(1));

            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
                while (await DispatchMessage(cloudTable, connectionName, queueName, scheduledScan, cancellationToken) && scheduledScan) ;
            } finally
            {
                semaphore.Release();
            }
        }

        private async Task<bool> DispatchMessage(CloudTable cloudTable, string connectionName, string queueName, bool scheduledScan, CancellationToken cancellationToken)
        {
            var statuses = new[] { TableMessageEntity.StatusDispatched, TableMessageEntity.StatusPending, TableMessageEntity.StatusSettings };
            var filter = $"(PartitionKey eq '{TableMessageEntity.CreatePartitionKey(queueName)}') and ({string.Join(" or ", statuses.Select(o => $"Status eq '{o}'"))})";
            var query = new TableQuery<TableMessageEntity>().Where(filter).Take(100);
            TableContinuationToken token = null;
            var results = (IEnumerable<TableMessageEntity>)(await cloudTable.ExecuteQuerySegmentedAsync(query, token, TableRequestOptions, OperationContext, cancellationToken));

            // find the settings row
            var settings = results.Where(o => o.IsSettings).Select(o => DeserializeSettings(o.Message)).FirstOrDefault();
            if (settings == null)
            {
                settings = await UpdateSettings(new AzTableQueueSettings()
                {
                    QueueName = queueName,
                    MaxConcurrentCalls = 1
                });
            }

            results = results.Where(o => !o.IsSettings);

            // update messages that have timed out
            if(scheduledScan)
            {
                await UpdateTimedOutMessages(cloudTable, settings, results, cancellationToken);
            }

            // dispatch new messages
            return await DispatchMessageAsync(cloudTable, settings, connectionName, results, cancellationToken);
        }

        private async Task<bool> UpdateTimedOutMessages(CloudTable cloudTable, AzTableQueueSettings settings, IEnumerable<TableMessageEntity> results, CancellationToken cancellationToken)
        {
            var timedOutMessage = results
                .Where(o => o.DispachedAt != null)
                .Where(o => o.DispachedAt.Value.Add(settings.Timeout) < DateTimeOffset.Now)
                .FirstOrDefault();
            if(timedOutMessage == null)
            {
                return false;
            }
            try
            {
                timedOutMessage.DispachedAt = null;
                timedOutMessage.Status = TableMessageEntity.StatusPending;
                await cloudTable.ExecuteAsync(TableOperation.Replace(timedOutMessage), TableRequestOptions, OperationContext, cancellationToken);
                return true;
            }
            catch (StorageException)
            {
                return false;
            }
        }

        private async Task<bool> DispatchMessageAsync(CloudTable cloudTable, AzTableQueueSettings settings, string connectionName, IEnumerable<TableMessageEntity> results, CancellationToken cancellationToken)
        {
            var maxConcurrentCalls = settings.MaxConcurrentCalls;
            var outstandingCalls = results.Where(o => o.Status == TableMessageEntity.StatusDispatched).Count();

            // try to dispatch another message
            var nextMessage = results.Where(o => o.Status == TableMessageEntity.StatusPending).FirstOrDefault();
            if (nextMessage == null)
            {
                // no more messages to dispatch
                return false;
            }

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"There are {outstandingCalls}/{maxConcurrentCalls} calls in progress among {results.Count()} calls when checking if {nextMessage.PartitionKey}:{nextMessage.RowKey} should be dispatched.");
            }

            if(maxConcurrentCalls <= outstandingCalls)
            {
                // we have reached the limit - do not try to dispatch more messages
                return false;
            }

            if(!(await ShouldDispatchMessage(cloudTable, nextMessage, cancellationToken))) 
            {
                // failed to dispatch this message - perhaps another process got before us
                return true;
            }

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Dispatching message({nextMessage.PartitionKey}:{nextMessage.RowKey}) to queue.");
            }

            // message locked and ready - add queue message
            await QueueInvoker.InvokeAsync(o => o.ProcessMessageAsync(connectionName, nextMessage.PartitionKey, nextMessage.RowKey, cancellationToken));

            // try to dispatch another message
            return true;
        }

        private async Task<bool> ShouldDispatchMessage(CloudTable cloudTable, TableMessageEntity nextMessage, CancellationToken cancellationToken)
        {
            nextMessage.Status = TableMessageEntity.StatusDispatched;
            nextMessage.DispachedAt = DateTimeOffset.Now;
            try
            {
                await cloudTable.ExecuteAsync(TableOperation.Replace(nextMessage), TableRequestOptions, OperationContext, cancellationToken);
                return true;
            }
            catch(StorageException se)
            {
                if (se.RequestInformation.HttpStatusCode == 412)
                {
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.LogTrace("Precondition failed when updating message row - trying another row.");
                    }
                    return false;
                }
                if (se.RequestInformation.HttpStatusCode == 404)
                {
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.LogTrace("Message row not found - trying another row.");
                    }
                    return false;
                }
                Logger.LogError(se, "Error trying to lock message.");
                return false;
                //throw;
            }
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

            HttpRequest httpRequest;
            SerializerFactory.DeserializeFromString(row.Message, out httpRequest);

            var request = new SolidHttpRequest();
            await request.CopyFromAsync(httpRequest);

            var resp = await MethodInvoker.InvokeAsync(ServiceProvider, request);
            if(resp.StatusCode >= 200 && resp.StatusCode < 300)
            {
                // row processed ok - delete it
                await cloudTable.ExecuteAsync(TableOperation.Delete(row), TableRequestOptions, OperationContext, cancellationToken);
            }
            else
            {
                row.Status = TableMessageEntity.StatusError;
                row.ProcessdBy = SolidRpcApplication.HostId;
                await cloudTable.ExecuteAsync(TableOperation.Replace(row), TableRequestOptions, OperationContext, cancellationToken);
            }

            await DispatchMessage(connectionName, row.QueueName, false, cancellationToken);
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
                var settings = DeserializeSettings(((TableMessageEntity)result?.Result)?.Message);
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

        private AzTableQueueSettings DeserializeSettings(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;
            AzTableQueueSettings settings;
            SerializerFactory.DeserializeFromString(message, out settings);
            return settings;
        }
        private string SerializeSettings(AzTableQueueSettings settings)
        {
            string message;
            SerializerFactory.SerializeToString(out message, settings);
            return message;
        }

        public async Task<AzTableQueueSettings> UpdateSettings(AzTableQueueSettings settings, CancellationToken cancellationToken = default)
        {
            var transport = GetTableTransports().FirstOrDefault(o => o.QueueName == settings.QueueName);
            if (transport == null) throw new ArgumentException("Cannot find transport for queue name.");
            var cloudTable = CloudQueueStore.GetCloudTable(transport.ConnectionName);
            var settingRow = new TableMessageEntity(settings.QueueName);
            settingRow.Message = SerializeSettings(settings);
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

        public Task SendTestMessageAync(int messageCount = 1, CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task>();
            for(int i = 0; i < messageCount; i++)
            {
                tasks.Add(QueueInvoker.InvokeAsync(o => o.ProcessTestMessage(cancellationToken)));
            }
            return Task.WhenAll(tasks);
        }

        public Task ProcessTestMessage(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
