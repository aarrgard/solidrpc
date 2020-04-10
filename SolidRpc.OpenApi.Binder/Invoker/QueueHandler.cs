using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    public abstract class QueueHandler : Handler
    {
        public QueueHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory,
            ISolidRpcApplication solidRpcApplication)
            :base(logger, serviceProvider)
        {
            SerializerFactory = serializerFactory;
            SolidRpcApplication = solidRpcApplication;
            var queueType = GetType().FullName.Split('.').Last();
            if(queueType.EndsWith("Handler"))
            {
                queueType = queueType.Substring(0, queueType.Length - "Handler".Length);
            }
            QueueType = queueType;
        }

        public ISerializerFactory SerializerFactory { get; }
        public ISolidRpcApplication SolidRpcApplication { get; }
        public string QueueType { get; }

        public override ITransport GetTransport(IEnumerable<ITransport> transports)
        {
            var queueTransports = transports.OfType<IQueueTransport>();
            var queueTransport = queueTransports.Where(o => o.QueueType == QueueType).FirstOrDefault();
            if(queueTransport == null)
            {
                throw new Exception($"Cannot find queue configuration for {QueueType} among configurations types {string.Join(",", queueTransports.Select(o => o.QueueType))}.");
            }
            return queueTransport;
        }

        protected override async Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, CancellationToken cancellationToken)
        {
            var httpReqData = new HttpRequest();
            await httpReq.CopyToAsync(httpReqData);

            string message;
            SerializerFactory.SerializeToString(out message, httpReqData);

            await SolidRpcApplication.WaitForStartupTasks();

            await InvokeAsync(methodBinding, (IQueueTransport)transport, message, cancellationToken);

            return new SolidHttpResponse()
            {
                StatusCode = 200
            };
        }

        protected abstract Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, CancellationToken cancellationToken);
    }
}
