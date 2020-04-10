using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.AzSvcBus.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;

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
            SvcBusHandler handler, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, handler, methodBinderStore, serviceProvider)
        {
        }
    }
}
