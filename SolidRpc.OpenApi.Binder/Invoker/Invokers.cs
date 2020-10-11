using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(Invokers), typeof(Invokers), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Class that caches invokers
    /// </summary>
    public class Invokers
    {
        public ConcurrentDictionary<Type, Func<MethodInfo, object[], InvocationOptions, object>> CachedInvokers = new ConcurrentDictionary<Type, Func<MethodInfo, object[], InvocationOptions, object>>();
        public Invokers(
            IMethodBinderStore methodBinderStore,
            IEnumerable<IHandler> handlers)
        {
            MethodBinderStore = methodBinderStore;
            Handlers = handlers;
        }
        public IMethodBinderStore MethodBinderStore { get; }
        public IEnumerable<IHandler> Handlers { get; }
    }
}
