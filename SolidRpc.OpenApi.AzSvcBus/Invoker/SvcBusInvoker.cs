using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AzSvcBus.Invoker;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IQueueInvoker<>), typeof(SvcBusInvoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.AzSvcBus.Invoker
{
    /// <summary>
    /// The queue invoker dispatches messages to the service bus.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SvcBusInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public SvcBusInvoker(
            ILogger<Invoker<T>> logger,
            ISolidRpcApplication solidRpcApplication,
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider,
            ISerializerFactory serializerFactory,
            IQueueClientStore queueClientStore) : base(logger, methodBinderStore, serviceProvider)
        {
            SolidRpcApplication = solidRpcApplication;
            SerializerFactory = serializerFactory;
            QueueClientStore = queueClientStore;
        }

        private ISolidRpcApplication SolidRpcApplication { get; }
        private ISerializerFactory SerializerFactory { get; }
        private IQueueClientStore QueueClientStore { get; }

        protected override async Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, MethodInfo mi, object[] args)
        {
            var messageId = Guid.NewGuid();
            try
            {
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

                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Started dispatching message {messageId}");
                }

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
                var ms = new MemoryStream();
                SerializerFactory.SerializeToStream(ms, httpReqData);

                // 4. Dispatch it.
                var msg = new Message(ms.ToArray());
                msg.MessageId = messageId.ToString();
                var qc = QueueClientStore.GetQueueClient(queueTransport.ConnectionName, queueTransport.QueueName);
                await qc.SendAsync(msg);

                //
                // Handle respose
                //
                var resultType = mi.ReturnType;
                if(resultType.IsTaskType(out Type taskType))
                {
                    resultType = taskType;
                }
                if(resultType.IsValueType)
                {
                    return Activator.CreateInstance(resultType);
                }
                return null;
            }
            finally
            {
                if(Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Done dispatching message {messageId}");
                }
            }
        }
    }
}
