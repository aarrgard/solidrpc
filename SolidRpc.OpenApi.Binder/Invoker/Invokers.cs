using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Concurrent;
using System.Reflection;

[assembly: SolidRpcService(typeof(Invokers), typeof(Invokers), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Class that caches invokers
    /// </summary>
    public class Invokers
    {
        public ConcurrentDictionary<Type, Func<MethodInfo, object[], InvocationOptions, object>> CachedInvokers = new ConcurrentDictionary<Type, Func<MethodInfo, object[], InvocationOptions, object>>();

    }
}
