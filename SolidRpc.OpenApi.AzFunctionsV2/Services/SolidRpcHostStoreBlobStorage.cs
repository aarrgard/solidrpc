using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzFunctionsV2Extension.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: SolidRpcService(typeof(ISolidRpcHostStore), typeof(SolidRpcHostStoreBlobStorage), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzFunctionsV2Extension.Services
{
    public class SolidRpcHostStoreBlobStorage : ISolidRpcHostStore
    {

        public SolidRpcHostStoreBlobStorage(ILogger<SolidRpcHostStoreBlobStorage> logger, IConfiguration configuration, ISerializerFactory serializerFactory)
        {
            Logger = logger;
            Configuration = configuration;
            SerializerFactory = serializerFactory;

            var storageConnectionString = Configuration["AzureWebJobsStorage"];
            if(!string.IsNullOrEmpty(storageConnectionString))
            {
                var sa = CloudStorageAccount.Parse(storageConnectionString);
            }
        }

        private ILogger Logger { get; }
        private IConfiguration Configuration { get; }
        private ISerializerFactory SerializerFactory { get; }

        public Task AddHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SolidRpcHostInstance>> ListHostInstancesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IEnumerable<SolidRpcHostInstance>>(new SolidRpcHostInstance[0]);
        }

        public Task RemoveHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
