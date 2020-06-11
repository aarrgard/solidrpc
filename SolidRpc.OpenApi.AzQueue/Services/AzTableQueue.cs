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
        private class RunningDispatch
        {
            public RunningDispatch(AzTableQueue azTableQueue, string key, string connectionName, string queueName)
            {
                AzTableQueue = azTableQueue;
                Key = key;
                ConnectionName = connectionName;
                QueueName = queueName;
                CancellationToken = CancellationToken.None;
                Task = DispatchMessageAsync();
            }
            public Task Task { get; }
            private AzTableQueue AzTableQueue { get; }
            private string Key { get; }
            private string ConnectionName { get; }
            private string QueueName { get; }
            private CancellationToken CancellationToken { get; }
            public bool ScheduledScan { get; set; }

            private async Task DispatchMessageAsync()
            {
                await Task.Yield(); // make sure that we add the task to the dictionary
                var semaphore = AzTableQueue.Semaphores.GetOrAdd(Key, _ => new SemaphoreSlim(1));
                
                //
                // Wait for previous task to complete
                //
                await semaphore.WaitAsync(CancellationToken);

                //
                // remove this instance from the pending dispatch set.
                //
                lock (AzTableQueue.PendingDispatches)
                {
                    var removed = AzTableQueue.PendingDispatches.Remove(new KeyValuePair<string, RunningDispatch>(Key, this));
                    if (!removed)
                    {
                        throw new Exception("Could not remove this instance.");
                    }
                }
                try
                {
                    var cloudTable = AzTableQueue.CloudQueueStore.GetCloudTable(ConnectionName);
                    if (ScheduledScan)
                    {
                        await AzTableQueue.UpdateErrorMessagesAsync(cloudTable, QueueName, CancellationToken);
                        await AzTableQueue.UpdateTimedOutMessagesAsync(cloudTable, QueueName, CancellationToken);
                    }

                    while (await AzTableQueue.DispatchMessageAsync(cloudTable, ConnectionName, QueueName, CancellationToken));
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        private static TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private static OperationContext OperationContext => new OperationContext();

        public static async Task<IEnumerable<TableMessageEntity>> GetMessagesAsync(CloudTable cloudTable, string queueName, IEnumerable<string> statuses, int takeCount, CancellationToken cancellationToken)
        {
            var filter = $"(PartitionKey eq '{TableMessageEntity.CreatePartitionKey(queueName)}') and ({string.Join(" or ", statuses.Select(o => $"Status eq '{o}'"))})";
            var query = new TableQuery<TableMessageEntity>().Where(filter).Take(takeCount);
            TableContinuationToken token = null;
            var lst = new List<TableMessageEntity>();
            do
            {
                var res = await cloudTable.ExecuteQuerySegmentedAsync(query, token, TableRequestOptions, OperationContext, cancellationToken);
                lst.AddRange(res.Results);
                token = res.ContinuationToken;
            } while (token != null && lst.Count < takeCount);
            return lst;
        }

        public AzTableQueue(
            ILogger<AzTableQueue> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            IInvoker<IAzTableQueue> invoker,
            IServiceProvider serviceProvider,
            AzTableHandler queueHandler,
            IMethodInvoker methodInvoker,
            ISolidRpcApplication solidRpcApplication)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            SerializerFactory = serializerFactory;
            CloudQueueStore = cloudQueueStore;
            Invoker = invoker;
            ServiceProvider = serviceProvider;
            QueueHandler = queueHandler;
            MethodInvoker = methodInvoker;
            SolidRpcApplication = solidRpcApplication;
            Semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
            PendingDispatches = new ConcurrentDictionary<string, RunningDispatch>();
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }
        private IInvoker<IAzTableQueue> Invoker { get; }
        private IServiceProvider ServiceProvider { get; }
        private AzTableHandler QueueHandler { get; }
        private IMethodInvoker MethodInvoker { get; }
        public ISolidRpcApplication SolidRpcApplication { get; }

        private ConcurrentDictionary<string, SemaphoreSlim> Semaphores { get; }
        private IDictionary<string, RunningDispatch> PendingDispatches { get; }

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

            var tasks = tableTranports.Select(o => DispatchMessageAsync(o.ConnectionName, o.QueueName, true, cancellationToken));
            return Task.WhenAll(tasks);
        }

        public Task DispatchMessageAsync(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            return DispatchMessageAsync(connectionName, queueName, false, cancellationToken);
        }

        private Task DispatchMessageAsync(string connectionName, string queueName, bool scheduledScan, CancellationToken cancellationToken)
        {
            //
            // Get/create pending dispatch
            //
            var key = $"{connectionName}:{queueName}";
            RunningDispatch runningDispatch;
            lock (PendingDispatches)
            {
                if(!PendingDispatches.TryGetValue(key, out runningDispatch))
                {
                    PendingDispatches[key] = runningDispatch = new RunningDispatch(this, key, connectionName, queueName);
                }
                if(scheduledScan)
                {
                    runningDispatch.ScheduledScan = true;
                }
            }

            return runningDispatch.Task;
        }

        private (IEnumerable<TableMessageEntity>, AzTableQueueSettings) GetSettings(IEnumerable<TableMessageEntity> results)
        {
            var settings = results.Where(o => o.IsSettings).Select(o => DeserializeSettings(o.Message)).FirstOrDefault();
            return (results = results.Where(o => !o.IsSettings), settings);
        }

        private async Task UpdateErrorMessagesAsync(CloudTable cloudTable, string queueName, CancellationToken cancellationToken)
        {
            var statuses = new[] { TableMessageEntity.StatusError, TableMessageEntity.StatusSettings };
            var results = await GetMessagesAsync(cloudTable, queueName, statuses, 1000, cancellationToken);
            var (rows, settings) = GetSettings(results);
            if (settings == null)
            {
                return;
            }
            var timedOutMessages = results
                .Where(o => o.DispachedAt != null)
                .Where(o => o.DispachedAt.Value.AddMinutes(Math.Pow(2, o.DispatchCount)) < DateTimeOffset.Now);
            foreach (var row in timedOutMessages)
            {
                try
                {
                    row.Status = TableMessageEntity.StatusPending;
                    row.DispachedAt = null;
                    await cloudTable.ExecuteAsync(TableOperation.Replace(row), TableRequestOptions, OperationContext, cancellationToken);
                }
                catch (StorageException)
                {
                }
            }
        }

        private async Task UpdateTimedOutMessagesAsync(CloudTable cloudTable, string queueName, CancellationToken cancellationToken)
        {
            var statuses = new[] { TableMessageEntity.StatusDispatched, TableMessageEntity.StatusSettings };
            var results = await GetMessagesAsync(cloudTable, queueName, statuses, 1000, cancellationToken);
            var (rows, settings) = GetSettings(results);
            if (settings == null)
            {
                return;
            }
            var timedOutMessages = results
                .Where(o => o.DispachedAt != null)
                .Where(o => o.DispachedAt.Value.Add(settings.Timeout) < DateTimeOffset.Now);
            foreach (var row in timedOutMessages)
            {
                try
                {
                    row.Status = TableMessageEntity.StatusPending;
                    row.DispachedAt = null;
                    await cloudTable.ExecuteAsync(TableOperation.Replace(row), TableRequestOptions, OperationContext, cancellationToken);
                }
                catch (StorageException)
                {
                }
            }
        }

        private async Task<bool> DispatchMessageAsync(CloudTable cloudTable, string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var statuses = new[] { TableMessageEntity.StatusDispatched, TableMessageEntity.StatusPending, TableMessageEntity.StatusSettings };
            var results2 = await GetMessagesAsync(cloudTable, queueName, statuses, 30, cancellationToken);
            var (results, settings) = GetSettings(results2);

            // find the settings row
            if (settings == null)
            {
                settings = await UpdateSettings(new AzTableQueueSettings()
                {
                    QueueName = queueName,
                    MaxConcurrentCalls = 1
                });
            }

            results = results.Where(o => !o.IsSettings);
            // dispatch new messages
            return await DispatchMessageAsync(cloudTable, settings, connectionName, results, cancellationToken);
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
            await Invoker.InvokeAsync(o => o.ProcessMessageAsync(connectionName, nextMessage.PartitionKey, nextMessage.RowKey, cancellationToken), InvocationOptions.AzQueue);

            // try to dispatch another message
            return true;
        }

        private async Task<bool> ShouldDispatchMessage(CloudTable cloudTable, TableMessageEntity nextMessage, CancellationToken cancellationToken)
        {
            nextMessage.DispatchCount = nextMessage.DispatchCount+1;
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

            await DispatchMessageAsync(connectionName, row.QueueName, false, cancellationToken);
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
                .Select(o => FlagErrorMessagesAsPending(o.ConnectionName, o.QueueName, cancellationToken));

            return Task.WhenAll(tasks);
        }

        private async Task FlagErrorMessagesAsPending(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var messages = await GetMessagesAsync(cloudTable, queueName, new[] { TableMessageEntity.StatusError }, int.MaxValue, cancellationToken);
            foreach(var message in messages)
            {
                message.Status = TableMessageEntity.StatusPending;
                await cloudTable.ExecuteAsync(TableOperation.Replace(message), TableRequestOptions, OperationContext, cancellationToken);
            }
        }
    }
}
