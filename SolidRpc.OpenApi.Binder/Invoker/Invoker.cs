using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    internal abstract class ResultConverter
    {
        protected static Func<Task<TFrom>, Task<TTo>> CreateTaskConverter<TFrom, TTo>()
        {
            return async _ =>
            {
                var t = (TTo)(object)(await _);
                return t;
            };
        }
        public ResultConverter()
        {
        }
        public Func<Task<object>, object> ConvertToTResult { get; protected set; }
        public Func<object, Task<object>> ConvertToObjectTask { get; protected set; }

    }
    internal class ResultConverter<TResult> : ResultConverter
    {
        public ResultConverter() : base()
        {
            if(typeof(TResult).IsTaskType(out Type taskType))
            {
                if(taskType == null)
                {
                    ConvertToTResult = _ => _;
                    ConvertToObjectTask = async _ => { await ((Task)_); return null; };
                }
                else
                {
                    ConvertToTResult = (Func<Task<object>, object>)typeof(ResultConverter)
                        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                        .Where(o => o.IsGenericMethod)
                        .Where(o => o.Name == nameof(CreateTaskConverter))
                        .Single().MakeGenericMethod(typeof(object), taskType)
                        .Invoke(null, null);

                    var x = (Func<TResult, Task<object>>)typeof(ResultConverter)
                        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                        .Where(o => o.IsGenericMethod)
                        .Where(o => o.Name == nameof(CreateTaskConverter))
                        .Single().MakeGenericMethod(taskType, typeof(object))
                        .Invoke(null, null);
                    ConvertToObjectTask = _ => x((TResult)_);
                }
            }
            else
            {
                ConvertToTResult = _ => _.Result;
                ConvertToObjectTask = _ => Task.FromResult<object>(_);
            }
        }
    }


    public abstract class Invoker<T> : IInvoker<T> where T:class
    {
 
        private static ConcurrentDictionary<Type, ResultConverter> s_ResultConverters = new ConcurrentDictionary<Type, ResultConverter>();
        private static readonly MethodInfo s_taskresultmethod = typeof(Invoker<T>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(o => o.IsGenericMethod)
            .Where(o => o.Name == nameof(ExtractTaskResult))
            .Single();

        public Invoker(ILogger<Invoker<T>> logger, IMethodBinderStore methodBinderStore, IServiceProvider serviceProvider)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            ServiceProvider = serviceProvider;
        }

        protected ILogger Logger { get; }
        protected IMethodBinderStore MethodBinderStore { get; }
        protected IServiceProvider ServiceProvider { get; }

        public Task InvokeAsync(Expression<Action<T>> action)
        {
            var (mi, args) = GetMethodInfo(action);
            var converter = s_ResultConverters.GetOrAdd(mi.ReturnType, CreateConverter);
            return InvokeMethodAsync(converter.ConvertToObjectTask, mi, args);
        }

        public TResult InvokeAsync<TResult>(Expression<Func<T, TResult>> func)
        {
            var (mi, args) = GetMethodInfo(func);
            var converter = s_ResultConverters.GetOrAdd(mi.ReturnType, CreateConverter);
            return (TResult)converter.ConvertToTResult(InvokeMethodAsync(converter.ConvertToObjectTask, mi, args));
        }

        public Task<object> InvokeAsync(MethodInfo mi, IEnumerable<object> args)
        {
            var converter = s_ResultConverters.GetOrAdd(mi.ReturnType, CreateConverter);
            return InvokeMethodAsync(converter.ConvertToObjectTask, mi, args.ToArray());
        }

        private ResultConverter CreateConverter(Type arg)
        {
            return (ResultConverter)Activator.CreateInstance(typeof(ResultConverter<>).MakeGenericType(arg));
        }

        private static (MethodInfo, object[]) GetMethodInfo(LambdaExpression expr)
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

        protected abstract Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, MethodInfo mi, object[] args);

        private async Task<object> ExtractNullResult(Task res)
        {
            await res;
            return null;
        }

        private async Task<object> ExtractTaskResult<TRes>(object res)
        {
            return await (Task<TRes>)res;
        }
    }
}
