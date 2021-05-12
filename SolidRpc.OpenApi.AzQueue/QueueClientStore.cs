using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzQueue;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ICloudQueueStore), typeof(CloudQueueStore), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzQueue
{
    public class CloudQueueStore : ICloudQueueStore
    {
        public CloudQueueStore(ILogger<CloudQueueStore> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            CloudQueues = new ConcurrentDictionary<string, ConcurrentDictionary<string, CloudQueue>>();
            CloudTables = new ConcurrentDictionary<string, ConcurrentDictionary<string, CloudTable>>();
            CloudBlobContainers = new ConcurrentDictionary<string, ConcurrentDictionary<string, CloudBlobContainer>>();
        }
        ILogger Logger { get; }
        IConfiguration Configuration { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudQueue>> CloudQueues { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudTable>> CloudTables { get; }
        private ConcurrentDictionary<string, ConcurrentDictionary<string, CloudBlobContainer>> CloudBlobContainers { get; }
        public CloudQueue GetCloudQueue(string connectionName, string queueName)
        {
            var dict = CloudQueues.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudQueue>());
            return dict.GetOrAdd(queueName, _ => CreateCloudQueue(connectionName, queueName));
        }

        public CloudTable GetCloudTable(string connectionName)
        {
            var tableName = "SolidRpcQueue";
            var dict = CloudTables.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudTable>());
            return dict.GetOrAdd(tableName, _ => CreateCloudTable(connectionName, tableName));
        }

        private CloudTable CreateCloudTable(string connectionName, string tableName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:" + connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference(tableName);
        }

        public CloudBlobContainer GetCloudBlobContainer(string connectionName)
        {
            var containerName = "solidrpcqueue";
            var dict = CloudBlobContainers.GetOrAdd(connectionName, _ => new ConcurrentDictionary<string, CloudBlobContainer>());
            return dict.GetOrAdd(containerName, _ => CreateCloudBlobContainer(connectionName, containerName));
        }

        private CloudBlobContainer CreateCloudBlobContainer(string connectionName, string containerName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:" + connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            return blobClient.GetContainerReference(containerName);
        }

        private CloudQueue CreateCloudQueue(string connectionName, string queueName)
        {
            var connectionString = Configuration[connectionName];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.Exception("Cannot find connection string for connection name:" + connectionName);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            var queueClient = storageAccount.CreateCloudQueueClient();

            return queueClient.GetQueueReference(queueName);
        }

        public async Task<string> StoreLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default)
        {
            // table storage stores messages in UTF16 max 64k
            var messageSize = message.Length * 2;
            if(messageSize <= (64*1024))
            {
                return message;
            }
            var guid = Guid.NewGuid();
            var blobContainer = GetCloudBlobContainer(connectionName);
            var blob = blobContainer.GetBlockBlobReference(guid.ToString());
            using (var s = await blob.OpenWriteAsync(new AccessCondition(), new BlobRequestOptions(), new OperationContext(), cancellationToken))
            using (var zs = new GZipStream(s, CompressionLevel.Optimal))
            {
                var enc = Encoding.UTF8;
                blob.Properties.ContentEncoding = enc.HeaderName;
                var arr = enc.GetBytes(message);
                await zs.WriteAsync(arr, 0, arr.Length);
            }
            return guid.ToString();
        }

        public async Task<string> RetreiveLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default)
        {
            if(!Guid.TryParse(message, out Guid guid))
            {
                return message;
            }
            try
            {
                var blobContainer = GetCloudBlobContainer(connectionName);
                var blob = blobContainer.GetBlockBlobReference(guid.ToString());
                using (var s = await blob.OpenReadAsync(new AccessCondition(), new BlobRequestOptions(), new OperationContext(), cancellationToken))
                using (var zs = new GZipStream(s, CompressionMode.Decompress))
                {
                    var enc = Encoding.GetEncoding(blob.Properties.ContentEncoding);
                    using (var sr = new StreamReader(zs, enc))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
            }
            catch(StorageException se)
            {
                if(se.RequestInformation.HttpStatusCode == 404)
                {
                    return null;
                }
                throw;
            }
        }

        public Task DeleteLargeMessageAsync(string connectionName, string message, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(message, out Guid guid))
            {
                return Task.CompletedTask;
            }
            var blobContainer = GetCloudBlobContainer(connectionName);
            var blob = blobContainer.GetBlockBlobReference(guid.ToString());
            return blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, new AccessCondition(), new BlobRequestOptions(), new OperationContext(), cancellationToken);
        }
    }
}
