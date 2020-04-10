using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;

namespace SolidRpc.OpenApi.AzQueue.Invoker
{
    /// <summary>
    /// The invoker that stores messages in an azure table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AzTableInvoker<T> : Invoker<T>, IQueueInvoker<T> where T : class
    {
        public AzTableInvoker(
            ILogger<Invoker<T>> logger, 
            AzTableHandler handler, 
            IMethodBinderStore methodBinderStore,
            IServiceProvider serviceProvider) 
            : base(logger, handler, methodBinderStore, serviceProvider)
        {
        }
    }
}
