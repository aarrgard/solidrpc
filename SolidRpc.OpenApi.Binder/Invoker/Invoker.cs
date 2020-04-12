using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    public abstract class Invoker<T> : IInvoker<T> where T:class
    {
        private static readonly IEnumerable<IHttpRequestData> EmptyCookieList = new IHttpRequestData[0];
        private static ConcurrentDictionary<Type, Func<SolidRpcHostInstance,MethodInfo,object[],object>> Invokers = new ConcurrentDictionary<Type, Func<SolidRpcHostInstance, MethodInfo, object[], object>>();
        public Invoker(ILogger<Invoker<T>> logger, IMethodBinderStore methodBinderStore, IServiceProvider serviceProvider)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            ServiceProvider = serviceProvider;
        }

        protected ILogger Logger { get; }
        protected IMethodBinderStore MethodBinderStore { get; }
        protected IServiceProvider ServiceProvider { get; }

        public IMethodBinding GetMethodBinding(MethodInfo mi)
        {
            return MethodBinderStore.GetMethodBinding(mi);
        }

        public IMethodBinding GetMethodBinding(Expression<Action<T>> action)
        {
            var (mi, _) = GetMethodInfo(action);
            return GetMethodBinding(mi);
        }

        public IMethodBinding GetMethodBinding<TResult>(Expression<Func<T, TResult>> func)
        {
            var (mi, _) = GetMethodInfo(func);
            return GetMethodBinding(mi);
        }
        public Task InvokeAsync(Expression<Action<T>> action, SolidRpcHostInstance targetInstance)
        {
            var (mi, args) = GetMethodInfo(action);
            return InvokeMethodAsync<object>(targetInstance, mi, args);
        }

        public TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func, SolidRpcHostInstance targetInstance)
        {
            var (mi, args) = GetMethodInfo(func);
            var res = Invokers.GetOrAdd(typeof(TResult), CreateInvoker<TResult>)(targetInstance, mi, args);
            return (TResult)res;
        }

        private Func<SolidRpcHostInstance, MethodInfo, object[], object> CreateInvoker<TResult>(Type t)
        {
            if(t.IsGenericType)
            {
                if(t.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var taskType = t.GetGenericArguments()[0];
                    var gmi = typeof(Invoker<T>).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                        .Where(o => o.Name == nameof(Narrow))
                        .Where(o => o.IsGenericMethod)
                        .Single();
                    gmi = gmi.MakeGenericMethod(taskType);
                    return (hi, mi, args) =>
                    {
                        var taskRes = InvokeAsync<TResult>(hi, mi, args);
                        var res = gmi.Invoke(this, new object[] { taskRes });
                        return res;
                    };
                }
            }
            return (hi, mi, args) =>
            {
                return InvokeAsync<TResult>(hi, mi, args).Result;
            };
        }
        
        private async Task<TResult> Narrow<TResult>(Task<Task<TResult>> result)
        {
            return await await result;
        }

        public Task<TResult> InvokeAsync<TResult>(SolidRpcHostInstance targetInstance, MethodInfo mi, IEnumerable<object> args)
        {
            return InvokeMethodAsync<TResult>(targetInstance, mi, args.ToArray());
        }

        public Task<object> InvokeAsync(SolidRpcHostInstance targetInstance, MethodInfo mi, IEnumerable<object> args)
        {
            return InvokeMethodAsync(targetInstance, mi, args.ToArray());
        }

        protected static (MethodInfo, object[]) GetMethodInfo(LambdaExpression expr)
        {
            if (expr.Body is MethodCallExpression mce)
            {
                var args = new List<object>();
                foreach (var argument in mce.Arguments)
                {
                    var le = Expression.Lambda(argument);
                    args.Add(le.Compile().DynamicInvoke());
                }

                return (mce.Method, args.ToArray());
            }
            throw new Exception("expression should be a method call.");
        }

        protected virtual Task<TRes> InvokeMethodAsync<TRes>(SolidRpcHostInstance targetInstance, MethodInfo mi, object[] args)
        {
            return GetHandler(mi).InvokeAsync<TRes>(mi, args);
        }

        protected virtual Task<object> InvokeMethodAsync(SolidRpcHostInstance targetInstance, MethodInfo mi, object[] args)
        {
            return GetHandler(mi).InvokeAsync<object>(mi, args);
        }

        private IHandler GetHandler(MethodInfo mi)
        {
            var binding = GetMethodBinding(mi);
            var handlers = ServiceProvider.GetRequiredService<IEnumerable<IHandler>>();
            var handler = FilterHandlers(handlers, binding);
            if(handler == null)
            {
                throw new Exception("Cannot find handler for method.");
            }
            return handler;
        }

        protected abstract IHandler FilterHandlers(IEnumerable<IHandler> handlers, IMethodBinding binding);
    }
}
