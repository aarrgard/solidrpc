using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: SolidRpcService(typeof(ILocalInvoker<>), typeof(LocalInvoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Invoker that uses a local implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocalInvoker<T> : Invoker<T>, ILocalInvoker<T> where T : class
    {
        public LocalInvoker(
            ILogger<Invoker<T>> logger, 
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, methodBinderStore, serviceProvider)
        {
        }


        protected override IHandler FilterHandlers(IEnumerable<IHandler> handlers, IMethodBinding binding)
        {
            return handlers.OfType<LocalHandler>().FirstOrDefault();
        }
    }
}
