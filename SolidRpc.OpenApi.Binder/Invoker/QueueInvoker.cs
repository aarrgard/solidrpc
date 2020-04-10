using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Abstract class that implements some of the queue logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class QueueInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public QueueInvoker(
            ILogger<Invoker<T>> logger, 
            QueueHandler handler, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, handler, methodBinderStore, serviceProvider)
        {
        }
    }
}
