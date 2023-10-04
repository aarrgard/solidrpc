using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
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

[assembly: SolidRpcService(typeof(ITransportHandler), typeof(AzTableHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
[assembly: SolidRpcService(typeof(AzTableHandler), typeof(AzTableHandler), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// Class responsible for doing the actual invocation.
    /// </summary>
    public class AzTableHandler : QueueHandler<IAzTableTransport>
    {
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
            public RunningDispatch(AzTableHandler azTableHandler, string connectionName, string queueName)
            {
                AzTableHandler = azTableHandler;
                ConnectionName = connectionName;
                QueueName = queueName;
                CancellationToken = CancellationToken.None;
                Semaphore = new SemaphoreSlim(1);
                Dispatching = new AsyncLocal<bool>();
            }
            private AzTableHandler AzTableHandler { get; }
            private string ConnectionName { get; }
            private string QueueName { get; }
            private CancellationToken CancellationToken { get; }
            private SemaphoreSlim Semaphore { get; }
            private AsyncLocal<bool> Dispatching { get; }

            public async Task DispatchMessageAsync(bool scheduledScan)
            {
                if(Dispatching.Value)
                {
                    return;
                }
                //
                // Wait for previous task to complete
                //
                if(!scheduledScan)
                {
                    if (!await Semaphore.WaitAsync(100, CancellationToken))
                    {
                        return;
                    }
                }
                try
                {
                    Dispatching.Value = true;
                    var cloudTable = AzTableHandler.CloudQueueStore.GetCloudTable(ConnectionName);
                    if (scheduledScan)
                    {
                        await AzTableHandler.UpdateErrorMessagesAsync(cloudTable, QueueName, CancellationToken);
                        await AzTableHandler.UpdateTimedOutMessagesAsync(cloudTable, QueueName, CancellationToken);
                    }

                    //
                    // since a job can wait for this to be completed we need to limit the number
                    // of jobs it dispatches. When this job finishes another task will do the rest
                    //
                    var dispatchCount = scheduledScan ? 1000 : 10; 
                    for(int i = 0; i < dispatchCount; i++)
                    {
                        bool dispatched = await AzTableHandler.DispatchMessageAsync(cloudTable, ConnectionName, QueueName, CancellationToken);
                        if (!dispatched)
                        {
                            break;
                        }
                    }
                }
                catch(Exception e)
                {
                    throw;
                }
                finally
                {
                    if (!scheduledScan)
                    {
                        Dispatching.Value = false;
                        Semaphore.Release();
                    }
                }
            }
        }


        public AzTableHandler(
            ILogger<QueueHandler<IAzTableTransport>> logger,
            IMethodBinderStore methodBinderStore,
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication,
            IServiceScopeFactory serviceScopeFactory) 
            : base(logger, methodBinderStore, serializerFactory, solidRpcApplication)
        {
            CloudQueueStore = cloudQueueStore;
            PendingDispatches = new ConcurrentDictionary<string, RunningDispatch>();
            ServiceScopeFactory = serviceScopeFactory;
        }
        private IDictionary<string, RunningDispatch> PendingDispatches { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private ICloudQueueStore CloudQueueStore { get; }

        public IEnumerable<IAzTableTransport> GetTableTransports()
        {
            // find all the table queues
            return MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .SelectMany(o => o.Transports)
                .OfType<IAzTableTransport>()
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

        protected override async Task InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, IAzTableTransport transport, string message, CancellationToken cancellationToken)
        {
            message = await CloudQueueStore.StoreLargeMessageAsync(transport.ConnectionName, message, cancellationToken);
            var priority = InvocationOptions.GetOptions(methodBinding.MethodInfo).Priority;
            if (priority <= 0) priority = 1;
            var msg = new TableMessageEntity(transport.QueueName, priority, message);
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
            var key = $"{connectionName}:{queueName}";
            lock (PendingDispatches)
            {
                if (!PendingDispatches.TryGetValue(key, out RunningDispatch runningDispatch))
                {
                    PendingDispatches[key] = runningDispatch = new RunningDispatch(this, connectionName, queueName);
                }
                return runningDispatch.DispatchMessageAsync(scheduledScan);
            }
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

        public async Task RemoveMessagesAsync(string connectionName, string queueName, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var statuses = new[] { TableMessageEntity.StatusDispatched, TableMessageEntity.StatusPending, TableMessageEntity.StatusError };
            IEnumerable<TableMessageEntity> rows = new TableMessageEntity[0];
            do
            {
                var batch = new TableBatchOperation();
                foreach(var row in rows)
                {
                    batch.Add(TableOperation.Delete(row));
                }
                if (batch.Any())
                {
                    await cloudTable.ExecuteBatchAsync(batch, TableRequestOptions, OperationContext, cancellationToken);
                }
                rows = await GetMessagesAsync(cloudTable, queueName, statuses, 100, cancellationToken);
            } while (rows.Any());
        }

        public async Task MoveMessagesAsync(string connectionName, string fromQueue, string toQueue, CancellationToken cancellationToken)
        {
            var cloudTable = CloudQueueStore.GetCloudTable(connectionName);
            var statuses = new[] { TableMessageEntity.StatusDispatched, TableMessageEntity.StatusPending, TableMessageEntity.StatusError };
            IEnumerable<TableMessageEntity> rows = new TableMessageEntity[0];
            do
            {
                foreach (var row in rows)
                {
                    var newRow = new TableMessageEntity(toQueue, row.Priority, row.Message);
                    try
                    {
                        await cloudTable.ExecuteAsync(TableOperation.Insert(newRow), TableRequestOptions, OperationContext, cancellationToken);
                        await cloudTable.ExecuteAsync(TableOperation.Delete(row), TableRequestOptions, OperationContext, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error moving message", e);
                    }
                }
                rows = await GetMessagesAsync(cloudTable, fromQueue, statuses, 100, cancellationToken);
            } while (rows.Any());
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

        public override void Configure(IMethodBinding methodBinding, IAzTableTransport transport)
        {
            base.Configure(methodBinding, transport);
        }
    }
}
