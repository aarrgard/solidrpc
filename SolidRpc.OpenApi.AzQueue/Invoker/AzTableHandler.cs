using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Services;
using SolidRpc.OpenApi.AzQueue.Types;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: SolidRpcService(typeof(IHandler), typeof(AzTableHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(AzTableHandler), typeof(AzTableHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// Class responsible for doing the actual invocation.
    /// </summary>
    public class AzTableHandler : QueueHandler
    {
        /// <summary>
        /// The transport type
        /// </summary>
        public static readonly new string TransportType = GetTransportType(typeof(AzTableHandler));

        private static TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private static OperationContext OperationContext => new OperationContext();

        internal AzTableQueueSettings DeserializeSettings(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;
            AzTableQueueSettings settings;
            SerializerFactory.DeserializeFromString(message, out settings);
            return settings;
        }
        internal string SerializeSettings(AzTableQueueSettings settings)
        {
            string message;
            SerializerFactory.SerializeToString(out message, settings);
            return message;
        }

        internal static async Task<IEnumerable<TableMessageEntity>> GetMessagesAsync(CloudTable cloudTable, string queueName, IEnumerable<string> statuses, int takeCount, CancellationToken cancellationToken)
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

        private class RunningDispatch
        {
            public RunningDispatch(AzTableHandler azTableHandler, string key, string connectionName, string queueName)
            {
                AzTableHandler = azTableHandler;
                Key = key;
                ConnectionName = connectionName;
                QueueName = queueName;
                CancellationToken = CancellationToken.None;
                Task = DispatchMessageAsync();
            }
            public Task Task { get; }
            private AzTableHandler AzTableHandler { get; }
            private string Key { get; }
            private string ConnectionName { get; }
            private string QueueName { get; }
            private CancellationToken CancellationToken { get; }
            public bool ScheduledScan { get; set; }

            private async Task DispatchMessageAsync()
            {
                await Task.Yield(); // make sure that we add the task to the dictionary
                var semaphore = AzTableHandler.Semaphores.GetOrAdd(Key, _ => new SemaphoreSlim(1));

                //
                // Wait for previous task to complete
                //
                await semaphore.WaitAsync(CancellationToken);

                //
                // remove this instance from the pending dispatch set.
                //
                lock (AzTableHandler.PendingDispatches)
                {
                    var removed = AzTableHandler.PendingDispatches.Remove(new KeyValuePair<string, RunningDispatch>(Key, this));
                    if (!removed)
                    {
                        throw new Exception("Could not remove this instance.");
                    }
                }
                try
                {
                    var cloudTable = AzTableHandler.CloudQueueStore.GetCloudTable(ConnectionName);
                    if (ScheduledScan)
                    {
                        await AzTableHandler.UpdateErrorMessagesAsync(cloudTable, QueueName, CancellationToken);
                        await AzTableHandler.UpdateTimedOutMessagesAsync(cloudTable, QueueName, CancellationToken);
                    }

                    while (await AzTableHandler.DispatchMessageAsync(cloudTable, ConnectionName, QueueName, CancellationToken)) ;
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }


        public AzTableHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication,
            IServiceScopeFactory serviceScopeFactory) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
            CloudQueueStore = cloudQueueStore;
            Semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
            PendingDispatches = new ConcurrentDictionary<string, RunningDispatch>();
            ServiceScopeFactory = serviceScopeFactory;
        }
        private ConcurrentDictionary<string, SemaphoreSlim> Semaphores { get; }
        private IDictionary<string, RunningDispatch> PendingDispatches { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }

        public IEnumerable<IQueueTransport> GetTableTransports()
        {
            // find all the table queues
            return MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .SelectMany(o => o.Transports)
                .OfType<IQueueTransport>()
                .Where(o => o.TransportType == TransportType)
                .ToList();
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
            catch (StorageException se)
            {
                throw;
            }
        }

        protected override async Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            message = await CloudQueueStore.StoreLargeMessageAsync(transport.ConnectionName, message, cancellationToken);
            var msg = new TableMessageEntity(transport.QueueName, invocationOptions.Priority, message);
            var tc = CloudQueueStore.GetCloudTable(transport.ConnectionName);
            await tc.ExecuteAsync(TableOperation.Insert(msg), TableRequestOptions, OperationContext, cancellationToken);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Sent message {msg.RowKey}");
            }

            await DispatchMessageAsync(transport.ConnectionName, transport.QueueName, false, cancellationToken);
        }


        public Task DispatchMessageAsync(string connectionName, string queueName, bool scheduledScan, CancellationToken cancellationToken)
        {
            //
            // Get/create pending dispatch
            // if we create a new dispatch we have to wait for it - otherwise we can return a completed task.
            //
            Task returnTask = Task.CompletedTask;
            var key = $"{connectionName}:{queueName}:{scheduledScan}";
            lock (PendingDispatches)
            {
                RunningDispatch runningDispatch;
                if (!PendingDispatches.TryGetValue(key, out runningDispatch))
                {
                    PendingDispatches[key] = runningDispatch = new RunningDispatch(this, key, connectionName, queueName);
                    runningDispatch.ScheduledScan = scheduledScan;
                    returnTask = runningDispatch.Task;
                }
            }

            return returnTask;
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
                .Where(o => o.DispachedAt.Value.AddMinutes(Math.Pow(2, Math.Min(o.DispatchCount, 20))) < DateTimeOffset.Now);
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
                }, cancellationToken);
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

            if (maxConcurrentCalls <= outstandingCalls)
            {
                // we have reached the limit - do not try to dispatch more messages
                return false;
            }

            if (!(await ShouldDispatchMessage(cloudTable, nextMessage, cancellationToken)))
            {
                // failed to dispatch this message - perhaps another process got before us
                return true;
            }

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Dispatching message({nextMessage.PartitionKey}:{nextMessage.RowKey}) to queue.");
            }

            // message locked and ready - add queue message
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                try
                {
                    var azTableQueue = scope.ServiceProvider.GetRequiredService<IAzTableQueue>();
                    await azTableQueue.ProcessMessageAsync(connectionName, nextMessage.PartitionKey, nextMessage.RowKey, cancellationToken);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error processing message - flagging it as error.");

                    var fetchResult = await cloudTable.ExecuteAsync(TableOperation.Retrieve<TableMessageEntity>(nextMessage.PartitionKey, nextMessage.RowKey), TableRequestOptions, OperationContext, cancellationToken);
                    var oldMessage = (TableMessageEntity)fetchResult.Result;
                    if (oldMessage.Status == TableMessageEntity.StatusDispatched)
                    {
                        oldMessage.Status = TableMessageEntity.StatusError;
                        await cloudTable.ExecuteAsync(TableOperation.Replace(oldMessage), TableRequestOptions, OperationContext, cancellationToken);
                    }
                }
            }

            // try to dispatch another message
            return true;
        }

        private async Task<bool> ShouldDispatchMessage(CloudTable cloudTable, TableMessageEntity nextMessage, CancellationToken cancellationToken)
        {
            nextMessage.DispatchCount = nextMessage.DispatchCount + 1;
            nextMessage.Status = TableMessageEntity.StatusDispatched;
            nextMessage.DispachedAt = DateTimeOffset.Now;
            try
            {
                await cloudTable.ExecuteAsync(TableOperation.Replace(nextMessage), TableRequestOptions, OperationContext, cancellationToken);
                return true;
            }
            catch (StorageException se)
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

        public async Task FlagErrorMessagesAsPending(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var messages = await GetMessagesAsync(cloudTable, queueName, new[] { TableMessageEntity.StatusError }, int.MaxValue, cancellationToken);
            foreach (var message in messages)
            {
                message.Status = TableMessageEntity.StatusPending;
                await cloudTable.ExecuteAsync(TableOperation.Replace(message), TableRequestOptions, OperationContext, cancellationToken);
            }
        }

    }
}
