using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IQueueInvoker<>), typeof(QueueInvoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// The queue invoker dispatches messages to the service bus.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public QueueInvoker(
            ILogger<Invoker<T>> logger,
            ISolidRpcApplication solidRpcApplication,
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider,
            ISerializerFactory serializerFactory,
            ICloudQueueStore queueClientStore) : base(logger, methodBinderStore, serviceProvider)
        {
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueClientStore = queueClientStore;
        }

        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ICloudQueueStore QueueClientStore { get; }

        protected override async Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, MethodInfo mi, object[] args)
        {
            try
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Started dispatching message...");
                }
                //
                // determine if we can dispatch the method invocation using the  
                // queue transport.
                //
                var methodBinding = MethodBinderStore.GetMethodBinding(mi);
                if (methodBinding == null)
                {
                    throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
                }
                var queueTransport = methodBinding.Transports.OfType<IQueueTransport>().FirstOrDefault();
                if (queueTransport == null)
                {
                    throw new Exception($"Queue transport not configured for method {mi.DeclaringType.FullName}.{mi.Name}");
                }

                //
                // The startup process may take longer than the first invocation
                // to dispatch a message - this is just to avoid annoying race conditions.
                //
                await SolidRpcApplication.WaitForStartupTasks();

                //
                // so we do
                // 1. Bind the arguments using the openapi spec
                // 2. Copy it to a representation that the serializer can use
                // 3. Serialize it
                // 4. Dispatch it.
                //

                // 1. Bind the arguments using the openapi spec
                var httpReq = new SolidHttpRequest();
                await methodBinding.BindArgumentsAsync(httpReq, args);
                AddSecurityKey(methodBinding, httpReq);

                // 2. Copy it to a representation that the serializer can use
                var httpReqData = new HttpRequest();
                await httpReq.CopyToAsync(httpReqData);

                // 3. Serialize it
                string message;
                SerializerFactory.SerializeToString(out message, httpReqData);

                // 4. Dispatch it.
                var msg = new CloudQueueMessage(message);
                var qc = QueueClientStore.GetQueueClient(queueTransport.ConnectionName, queueTransport.QueueName);
                await qc.AddMessageAsync(msg);
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Sent message {msg.Id}");
                }

                //
                // Handle respose
                //
                var resultType = mi.ReturnType;
                if(resultType.IsTaskType(out Type taskType))
                {
                    resultType = taskType;
                }
                if(resultType?.IsValueType ?? false)
                {
                    return Activator.CreateInstance(resultType);
                }
                return null;
            }
            finally
            {
                if(Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"...done dispatching message.");
                }
            }
        }
    }
}
