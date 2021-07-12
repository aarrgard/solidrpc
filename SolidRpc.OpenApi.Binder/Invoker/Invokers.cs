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
        private ConcurrentDictionary<Type, Func<IMethodBinding, object, MethodInfo, object[], Func<InvocationOptions, InvocationOptions>, object>> CachedInvokers = new ConcurrentDictionary<Type, Func<IMethodBinding, object, MethodInfo, object[], Func<InvocationOptions, InvocationOptions>, object>>();
        public Invokers(
            IMethodBinderStore methodBinderStore,
            IEnumerable<IHandler> handlers)
        {
            MethodBinderStore = methodBinderStore;
            Handlers = handlers;
        }
        public IMethodBinderStore MethodBinderStore { get; }
        public IEnumerable<IHandler> Handlers { get; }

        public Func<IMethodBinding, object, MethodInfo, object[], Func<InvocationOptions, InvocationOptions>, object> GetCachedInvoker<TResult>()
        {
            return CachedInvokers.GetOrAdd(typeof(TResult), CreateInvoker<TResult>);
        }

        public Func<IMethodBinding, object, MethodInfo, object[], Func<InvocationOptions, InvocationOptions>, object> CreateInvoker<TResult>(Type t)
        {
            if (t.IsTaskType(out Type taskType))
            {
                if (taskType == null)
                {
                    taskType = typeof(object);
                }
                var gmi = typeof(Invokers).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(o => o.Name == nameof(Narrow))
                    .Where(o => o.IsGenericMethod)
                    .Single();
                gmi = gmi.MakeGenericMethod(taskType);
                return (methodBinding, proxy, mi, args, opts) =>
                {
                    var taskRes = InvokeMethodAsync(methodBinding, proxy, mi, args, opts);
                    var res = gmi.Invoke(this, new object[] { taskRes });
                    return res;
                };
            }
            return (methodBinding, proxy, mi, args, opts) =>
            {
                return InvokeMethodAsync(methodBinding, proxy, mi, args, opts).Result;
            };
        }

        private async Task<TResult> Narrow<TResult>(Task<object> result)
        {
            return (TResult)(await result);
        }

        public virtual Task<object> InvokeMethodAsync(IMethodBinding methodBinding, object proxy, MethodInfo mi, object[] args, Func<InvocationOptions, InvocationOptions> invocationOptions)
        {
            var transportType = MethodBinderStore.GetMethodBinding(mi)?.Transports
                    .OrderBy(o => o.InvocationStrategy)
                    .First().TransportType ?? InvocationOptions.Local.TransportType;
            var transformedOptions = new InvocationOptions(transportType, InvocationOptions.MessagePriorityNormal);
            transformedOptions = invocationOptions?.Invoke(transformedOptions) ?? transformedOptions;

            var handler = Handlers.FirstOrDefault(o => o.TransportType == transformedOptions.TransportType);
            if (handler == null) throw new Exception($"Transport {transportType} not configured.");
            return handler.InvokeAsync(methodBinding, proxy, mi, args, transformedOptions);
        }
    }
}
