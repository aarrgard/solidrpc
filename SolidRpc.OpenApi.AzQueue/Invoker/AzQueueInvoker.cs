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

[assembly: SolidRpcService(typeof(IQueueInvoker<>), typeof(AzQueueInvoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// The queue invoker dispatches messages to the service bus.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AzQueueInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public AzQueueInvoker(
            ILogger<Invoker<T>> logger, 
            AzQueueHandler handler, 
            IMethodBinderStore methodBinderStore,
            IServiceProvider serviceProvider) 
            : base(logger, handler, methodBinderStore, serviceProvider)
        {
        }
    }
}
