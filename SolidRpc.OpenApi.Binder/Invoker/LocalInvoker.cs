using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

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
            LocalHandler handler,
            IMethodBinderStore methodBinderStore, 
            IServiceProvider serviceProvider) 
            : base(logger, handler, methodBinderStore, serviceProvider)
        {
        }
    }
}
