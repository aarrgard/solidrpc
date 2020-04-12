﻿using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: SolidRpcService(typeof(IHandler), typeof(AzTableHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// Class responsible for doing the actual invocation.
    /// </summary>
    public class AzTableHandler : QueueHandler
    {
        public AzTableHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory,
            ICloudQueueStore cloudQueueStore,
            ISolidRpcApplication solidRpcApplication) 
            : base(logger, serviceProvider, serializerFactory, solidRpcApplication)
        {
            CloudQueueStore = cloudQueueStore;
        }

        private ICloudQueueStore CloudQueueStore { get; }
        private TableRequestOptions TableRequestOptions => new TableRequestOptions();
        private OperationContext OperationContext => new OperationContext();

        protected override async Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, CancellationToken cancellationToken)
        {
            var msg = new TableMessageEntity(transport.QueueName, 5, message);
            var tc = CloudQueueStore.GetCloudTable(transport.ConnectionName);
            await tc.ExecuteAsync(TableOperation.Insert(msg), TableRequestOptions, OperationContext, cancellationToken);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Sent message {msg.RowKey}");
            }
        }
    }
}