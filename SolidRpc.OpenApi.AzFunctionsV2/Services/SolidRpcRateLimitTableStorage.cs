using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services.RateLimit;
using SolidRpc.Abstractions.Types.RateLimit;
using SolidRpc.OpenApi.AzFunctionsV2Extension.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: SolidRpcService(typeof(ISolidRpcRateLimit), typeof(SolidRpcRateLimitTableStorage), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzFunctionsV2Extension.Services
{
    public class SolidRpcRateLimitTableStorage : ISolidRpcRateLimit
    {
        private CloudTable _cloudTable;

        public SolidRpcRateLimitTableStorage(ILogger<SolidRpcRateLimitTableStorage> logger, IConfiguration configuration, ISerializerFactory serializerFactory)
        {
            Logger = logger;
            Configuration = configuration;
            SerializerFactory = serializerFactory;

            ResoureSemaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
            var storageConnectionString = Configuration["AzureWebJobsStorage"];
            if(!string.IsNullOrEmpty(storageConnectionString))
            {
                var sa = CloudStorageAccount.Parse(storageConnectionString);
                CloudTableClient = sa.CreateCloudTableClient();
            }
        }

        private ILogger Logger { get; }
        private IConfiguration Configuration { get; }
        private ISerializerFactory SerializerFactory { get; }
        private CloudTableClient CloudTableClient { get; }
        private ConcurrentDictionary<string, SemaphoreSlim> ResoureSemaphores { get; }

        private TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private OperationContext OperationContext => new OperationContext();

        private async Task<CloudTable> GetCloudTableAsync(CancellationToken cancellationToken)
        {
            if (_cloudTable != null) return _cloudTable;
            if (CloudTableClient == null) return null;
            var cloudTable = CloudTableClient.GetTableReference("SolidRpcRateLimit");
            await cloudTable.CreateIfNotExistsAsync();
            return _cloudTable = cloudTable;
        }

        public Task<RateLimitToken> GetSingeltonTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetRateLimitTokenAsync(resourceName, timeout, true, cancellationToken);
        }

        public Task<RateLimitToken> GetRateLimitTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetRateLimitTokenAsync(resourceName, timeout, false, cancellationToken);
        }

        public async Task<RateLimitToken> GetRateLimitTokenAsync(string resourceName, TimeSpan timeout, bool singelton, CancellationToken cancellationToken = default(CancellationToken))
        {
            var timedOut = DateTime.Now.Add(timeout);

            //
            // make sure that there is only one call terrorising the table storage
            //
            var resourceSemaphore = ResoureSemaphores.GetOrAdd(resourceName, _ => new SemaphoreSlim(1));
            var canEnter = await resourceSemaphore.WaitAsync(timeout, cancellationToken);
            if(!canEnter) 
            {
                return new RateLimitToken();
            }

            try
            {
                var cloudTable = await GetCloudTableAsync(cancellationToken);
                if (cloudTable == null) return new RateLimitToken();

                //
                // insert a new row in the table.
                //
                var rateLimitToken = new RateLimitToken()
                {
                    ResourceName = resourceName,
                    Id = Guid.NewGuid(),
                    Expires = DateTimeOffset.Now.AddMinutes(1)
                };

                try
                {
                    var entity = new SolidRpcRateLimitEntity(rateLimitToken.ResourceName, rateLimitToken.Id.ToString());
                    entity.Expires = rateLimitToken.Expires;
                    await cloudTable.ExecuteAsync(TableOperation.Insert(entity), TableRequestOptions, OperationContext, cancellationToken);

                    do
                    {
                        var nbrAhead = await CanEnter(cloudTable, resourceName, rateLimitToken.Id, singelton, cancellationToken);
                        if (nbrAhead <= 0)
                        {
                            return rateLimitToken;
                        }
                        await Task.Delay(nbrAhead * 50);
                    } while (timedOut > DateTime.Now);

                    // remove the token from table storage and return empty token.
                    await ReturnRateLimitTokenAsync(rateLimitToken, cancellationToken);
                    return new RateLimitToken();
                }
                catch (Exception e)
                {
                    // make sure that we return the token
                    await ReturnRateLimitTokenAsync(rateLimitToken, CancellationToken.None);
                    throw;
                }
            }
            finally
            {
                resourceSemaphore.Release();
            }
        }

        private async Task<int> CanEnter(CloudTable cloudTable, string resourceName, Guid id, bool singelton, CancellationToken cancellationToken)
        {
            //
            // list the rows in the table - should include the settings
            //
            TableContinuationToken ct = null;
            var query = new TableQuery<SolidRpcRateLimitEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, resourceName));
            var results = await cloudTable.ExecuteQuerySegmentedAsync(query, ct, TableRequestOptions, OperationContext, cancellationToken);
            RateLimitSetting settings = null;
            var rlts = new List<RateLimitToken>();
            foreach (var result in results.Results.OrderBy(o => o.Timestamp))
            {
                if (result.RowKey == "settings")
                {
                    SerializerFactory.DeserializeFromString(result.Json, out settings);
                }
                else
                {
                    rlts.Add(new RateLimitToken()
                    {
                        ResourceName = result.PartitionKey,
                        Id = Guid.Parse(result.RowKey),
                        Expires = result.Expires.Value
                    });
                }
            }

            //
            // return the tokens that has expired
            //
            var expiredTokens = rlts.Where(o => o.Expires < DateTimeOffset.Now).ToList();
            var returnTasks = expiredTokens.Select(o => ReturnRateLimitTokenAsync(o, cancellationToken));
            await Task.WhenAll(returnTasks);
            rlts.RemoveAll(o => expiredTokens.Contains(o));

            var nbrAhead = 0;

            //
            // find index of this token.
            // 
            var thisRateLimitToken = rlts.FirstOrDefault(o => o.Id == id);
            if(thisRateLimitToken == null)
            {
                return nbrAhead;
            }
            var idx = rlts.IndexOf(thisRateLimitToken);


            if (settings == null)
            {
                if(singelton)
                {
                    settings = new RateLimitSetting()
                    {
                        ResourceName = resourceName,
                        MaxConcurrentCalls = 1
                    };
                }
                else
                {
                    settings = new RateLimitSetting()
                    {
                        ResourceName = resourceName
                    };
                    await UpdateRateLimitSetting(settings, cancellationToken);
                }
            }

            if (settings.MaxConcurrentCalls != null)
            {
                nbrAhead = 1 + idx - settings.MaxConcurrentCalls.Value;
            }

            return nbrAhead;
        }

        public async Task ReturnRateLimitTokenAsync(RateLimitToken rateLimitToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cloudTable = await GetCloudTableAsync(cancellationToken);
            if (cloudTable == null) return;
            var entity = new SolidRpcRateLimitEntity(rateLimitToken.ResourceName, rateLimitToken.Id.ToString());
            try
            {
                await cloudTable.ExecuteAsync(TableOperation.Delete(entity), TableRequestOptions, OperationContext, cancellationToken);
            } 
            catch(StorageException se)
            {
                if(se.RequestInformation.HttpStatusCode == 404)
                {
                    return;
                }
                throw;
            }
        }

        public async Task<IEnumerable<RateLimitSetting>> GetRateLimitSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var cloudTable = await GetCloudTableAsync(cancellationToken);
            if (cloudTable == null) return new RateLimitSetting[0];

            TableContinuationToken ct = null;
            var query = new TableQuery<SolidRpcRateLimitEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "settings"));
            var results = await cloudTable.ExecuteQuerySegmentedAsync(query, ct, TableRequestOptions, OperationContext, cancellationToken);
            var retVal = new List<RateLimitSetting>();
            foreach (var result in results.Results)
            {
                RateLimitSetting settings;
                SerializerFactory.DeserializeFromString(result.Json, out settings);
                retVal.Add(settings);
            }
            return retVal;
        }

        public async Task UpdateRateLimitSetting(RateLimitSetting setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cloudTable = await GetCloudTableAsync(cancellationToken);
            if (cloudTable == null) return;

            var entity = new SolidRpcRateLimitEntity(setting.ResourceName, "settings");
            string json;
            SerializerFactory.SerializeToString(out json, setting);
            entity.Json = json;

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Updating settings for rate limit:" + json);
            }

            await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(entity), TableRequestOptions, OperationContext, cancellationToken);
        }
    }

    internal class SolidRpcRateLimitEntity : TableEntity
    {
        public SolidRpcRateLimitEntity()
        {
        }

        public SolidRpcRateLimitEntity(string scope, string id)
        {
            PartitionKey = scope;
            RowKey = id;
            ETag = "*";
        }

        public string Json { get; set; }
        public DateTimeOffset? Expires { get;  set; }
    }
}
