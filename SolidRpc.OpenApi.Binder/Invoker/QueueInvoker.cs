using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: SolidRpcService(typeof(IQueueInvoker<>), typeof(QueueInvoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Abstract class that implements some of the queue logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public QueueInvoker(
            ILogger<Invoker<T>> logger, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, methodBinderStore, serviceProvider)
        {
        }

        protected override IHandler FilterHandlers(IEnumerable<IHandler> handlers, IMethodBinding binding)
        {
            var queueTypes = new HashSet<string>(binding.Transports.OfType<IQueueTransport>().Select(o => o.QueueType));
            var queueHandlers = handlers.OfType<QueueHandler>();
            queueHandlers = queueHandlers.Where(o => queueTypes.Contains(o.QueueType));
            return queueHandlers.FirstOrDefault();
        }
    }
}
