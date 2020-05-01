using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IInvoker<>), typeof(Invoker<>), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    public class Invoker<T> : IInvoker<T> where T:class
    {
        private static readonly IEnumerable<IHttpRequestData> EmptyCookieList = new IHttpRequestData[0];
        private static ConcurrentDictionary<Type, Func<MethodInfo,object[], InvocationOptions, object>> Invokers = new ConcurrentDictionary<Type, Func<MethodInfo, object[], InvocationOptions, object>>();
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

        public Task<Uri> GetUriAsync(Expression<Action<T>> action, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(action);
            return GetUriAsync(mi, args, includeQueryString);
        }

        public Task<Uri> GetUriAsync<TRes>(Expression<Func<T, TRes>> func, bool includeQueryString = true)
        {
            var (mi, args) = GetMethodInfo(func);
            return GetUriAsync(mi, args, includeQueryString);
        }

        private async Task<Uri> GetUriAsync(MethodInfo mi, object[] args, bool includeQueryString)
        {
            var methodBinding = GetMethodBinding(mi);
            if (methodBinding == null)
            {
                throw new Exception($"Cannot find openapi method binding for method {mi.DeclaringType.FullName}.{mi.Name}");
            }
            var operationAddress = methodBinding.Transports
                .Where(o => o.TransportType == "Http")
                .Select(o => o.OperationAddress)
                .FirstOrDefault();

            var req = new SolidHttpRequest();
            await methodBinding.BindArgumentsAsync(req, args, operationAddress);
            return req.CreateUri(includeQueryString);
        }

        public Task InvokeAsync(Expression<Action<T>> action, InvocationOptions invocationOptions)
        {
            var (mi, args) = GetMethodInfo(action);
            return InvokeMethodAsync<object>(mi, args, invocationOptions);
        }

        public TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func, InvocationOptions invocationOptions)
        {
            var (mi, args) = GetMethodInfo(func);
            var res = Invokers.GetOrAdd(typeof(TResult), CreateInvoker<TResult>)(mi, args, invocationOptions);
            return (TResult)res;
        }

        private Func<MethodInfo, object[], InvocationOptions , object> CreateInvoker<TResult>(Type t)
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
                    return (mi, args, opts) =>
                    {
                        var taskRes = InvokeAsync<TResult>(mi, args, opts);
                        var res = gmi.Invoke(this, new object[] { taskRes });
                        return res;
                    };
                }
            }
            return (mi, args, opts) =>
            {
                return InvokeAsync<TResult>(mi, args, opts).Result;
            };
        }
        
        private async Task<TResult> Narrow<TResult>(Task<Task<TResult>> result)
        {
            return await await result;
        }

        public Task<TResult> InvokeAsync<TResult>(MethodInfo mi, IEnumerable<object> args, InvocationOptions invocationOptions)
        {
            return InvokeMethodAsync<TResult>(mi, args.ToArray(), invocationOptions);
        }

        public Task<object> InvokeAsync(MethodInfo mi, IEnumerable<object> args, InvocationOptions invocationOptions)
        {
            return InvokeMethodAsync(mi, args.ToArray(), invocationOptions);
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

        protected virtual Task<TRes> InvokeMethodAsync<TRes>(MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            return GetHandler(mi, ref invocationOptions).InvokeAsync<TRes>(mi, args, invocationOptions);
        }

        protected virtual Task<object> InvokeMethodAsync(MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            return GetHandler(mi, ref invocationOptions).InvokeAsync<object>(mi, args, invocationOptions);
        }

        private IHandler GetHandler(MethodInfo mi, ref InvocationOptions invocationOptions)
        {
            var transportType = invocationOptions?.TransportType;
            if(transportType == null)
            {
                transportType = GetMethodBinding(mi).Transports
                    .OrderBy(o => o.InvocationStrategy)
                    .First().TransportType;
                invocationOptions = new InvocationOptions(transportType, InvocationOptions.MessagePriorityNormal);
            }

            var handlers = ServiceProvider.GetRequiredService<IEnumerable<IHandler>>();
            var handler = handlers.FirstOrDefault(o => o.TransportType == transportType);
            if (handler == null) throw new Exception($"Transport {transportType} not configured.");
            return handler;
        }
    }
}
