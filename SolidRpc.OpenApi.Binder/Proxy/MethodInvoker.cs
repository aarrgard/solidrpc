using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Configuration.Runtime;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IMethodInvoker), typeof(MethodInvoker))]
[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IMethodInvoker<>), typeof(MethodInvoker<>))]
namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// The implementation of the method invoker.
    /// </summary>
    public class MethodInvoker : IMethodInvoker
    {
        private class PathSegment
        {
            public PathSegment()
            {
                SubSegments = new Dictionary<string, PathSegment>();
            }
            public PathSegment(PathSegment parentSegment) : this()
            {
                ParentSegment = parentSegment;
            }

            public PathSegment ParentSegment { get; }

            private IDictionary<string, PathSegment> SubSegments { get; }

            public IMethodBinding MethodInfo { get; set; }

            public void AddPath(IMethodBinding methodInfo)
            {
                var work = this;
                var segments = $"{methodInfo.Method}{methodInfo.Address.LocalPath}".Split('/');
                foreach (var segment in segments)
                {
                    var key = segment;
                    if (key.StartsWith("{"))
                    {
                        key = "{}";
                    }
                    if (!work.SubSegments.TryGetValue(key, out PathSegment subSegment))
                    {
                        work.SubSegments[key] = subSegment = new PathSegment(work);
                    }
                    work = subSegment;
                }
                work.MethodInfo = methodInfo;
            }

            public IMethodBinding GetMethodInfo(IEnumerator<string> segments)
            {
                if (!segments.MoveNext())
                {
                    return MethodInfo;
                }
                if (!SubSegments.TryGetValue(segments.Current, out PathSegment subSegment))
                {
                    if (!SubSegments.TryGetValue("{}", out subSegment))
                    {
                        throw new Exception($"Failed to find segment {segments.Current} among segments {string.Join(",", SubSegments.Keys)}");
                    }
                }
                return subSegment.GetMethodInfo(segments);
            }
        }

        private readonly object _mutex = new object();
        private PathSegment _rootSegment;

        public MethodInvoker(
            ILogger<MethodInvoker> logger,
            IServiceProvider serviceProvider,
            IMethodBinderStore methodBinderStore,
            ISolidProxyConfigurationStore proxyConfigurationStore)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            ProxyConfigurationStore = proxyConfigurationStore;
            MethodBinderStore = methodBinderStore;
            MethodInfo2Binding = new Dictionary<MethodInfo, IMethodBinding>();
        }

        public ILogger Logger { get; }
        public IServiceProvider ServiceProvider { get; }
        public ISolidProxyConfigurationStore ProxyConfigurationStore { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        public Dictionary<MethodInfo, IMethodBinding>  MethodInfo2Binding { get; }
        private PathSegment RootSegment => GetRootSegment();

        private PathSegment GetRootSegment()
        {
            if(_rootSegment == null)
            {
                lock(_mutex)
                {
                    if(_rootSegment == null)
                    {
                        _rootSegment = new PathSegment();
                        MethodBinderStore.MethodBinders
                            .SelectMany(o => o.MethodBindings)
                            .ToList().ForEach(methodBinding =>
                        {
                            _rootSegment.AddPath(methodBinding);
                            MethodInfo2Binding.Add(methodBinding.MethodInfo, methodBinding);
                            var mi = methodBinding.MethodInfo;
                            Logger.LogInformation($"Added {mi.DeclaringType.FullName}.{mi.Name}@{methodBinding.Address.LocalPath}.");
                        });
                    }
                }
            }
            return _rootSegment;
        }

        public Task<IHttpResponse> InvokeAsync(
            IHttpRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var pathKey = $"{request.Method}{request.Path}";
            var methodInfo = RootSegment.GetMethodInfo(pathKey.Split('/').AsEnumerable().GetEnumerator());
            if(methodInfo == null)
            {
                Logger.LogError("Could not find mapping for path " + pathKey);
                return Task.FromResult<IHttpResponse>(new SolidHttpResponse()
                {
                    StatusCode = 404
                });
            }
            return InvokeAsync(request, methodInfo, cancellationToken);
        }

        public async Task<IHttpResponse> InvokeAsync(
            IHttpRequest request, 
            IMethodBinding methodInfo, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogInformation($"Method invoker handling http request:{request.Scheme}://{request.HostAndPort}{request.Path}");
            var args = await methodInfo.ExtractArgumentsAsync(request);

            // invoke
            var svc = ServiceProvider.GetService(methodInfo.MethodInfo.DeclaringType);
            if (svc == null)
            {
                throw new Exception($"Failed to resolve service for type {methodInfo.MethodInfo.DeclaringType}");
            }
            var proxy = (ISolidProxy)svc;
            if (proxy == null)
            {
                throw new Exception($"Service for {methodInfo.MethodInfo.DeclaringType} is not a solid proxy.");
            }

            var invocationValues = new Dictionary<string, object>();
            foreach(var qv in request.Headers)
            {
                if(invocationValues.TryGetValue(qv.Name, out object value))
                {
                    invocationValues[qv.Name] = StringValues.Concat((StringValues)value, qv.GetStringValue());
                }
                else
                {
                    invocationValues.Add(qv.Name, new StringValues(qv.GetStringValue()));
                }
            }

            var resp = new SolidHttpResponse();
            try
            {
                var res = await proxy.InvokeAsync(methodInfo.MethodInfo, args, invocationValues);

                // return response
                //
                // the InvokeAsync never returns a Task<T>. Strip off the task type if exists...
                //
                var resType = methodInfo.MethodInfo.ReturnType;
                if (resType.IsTaskType(out Type taskType))
                {
                    resType = taskType ?? resType;
                }
                //
                // bind the response
                //
                await methodInfo.BindResponseAsync(resp, res, resType);
            }
            catch (Exception ex)
            {
                // handle exception
                Logger.LogError(ex, "Service returned an exception - sending to client");
                await methodInfo.BindResponseAsync(resp, ex, methodInfo.MethodInfo.ReturnType);
            }
            return resp;

        }

        public async Task<object> InvokeInternalAsync(MethodInfo methodInfo, IEnumerable<object> args, CancellationToken cancellation = default(CancellationToken))
        {
            var svc = ServiceProvider.GetService(methodInfo.DeclaringType);
            if (svc == null)
            {
                throw new Exception($"Failed to resolve service for type {methodInfo.DeclaringType}");
            }
            var proxy = (ISolidProxy)svc;
            if(proxy == null)
            {
                var result = methodInfo.Invoke(svc, args.ToArray());
                var resultTask = result as Task;
                if (resultTask != null)
                {
                    await resultTask;
                    return null;
                }
                return result;
            }

            //
            // Find/add security key
            //
            IDictionary<string, object> invocationValues = null;
            GetRootSegment();
            if(MethodInfo2Binding.TryGetValue(methodInfo, out IMethodBinding binding))
            {
                var securityKey = binding.SecurityKey;
                if (securityKey != null)
                {
                    invocationValues = new Dictionary<string, object>() 
                    {
                        { securityKey.Value.Key, new StringValues(securityKey.Value.Value) }
                    };
                }
            }
            var res = await proxy.InvokeAsync(methodInfo, args?.ToArray(), invocationValues);
            return res;
        }
    }

    /// <summary>
    /// Implements the method invoker
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MethodInvoker<T> : MethodInvoker, IMethodInvoker<T>
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="proxyConfigurationStore"></param>
        public MethodInvoker(ILogger<MethodInvoker<T>> logger, IServiceProvider serviceProvider, IMethodBinderStore methodBinderStore, ISolidProxyConfigurationStore proxyConfigurationStore) : base(logger, serviceProvider, methodBinderStore, proxyConfigurationStore)
        {
        }

        /// <summary>
        /// Invokes the action
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task InvokeInternalAsync<TResult>(Expression<Action<T>> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var (mi, args) = GetMethodInfo(action);
            return InvokeInternalAsync(mi, args, cancellationToken);
        }

        /// <summary>
        /// Invokes the function
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public TResult InvokeInternalAsync<TResult>(Expression<Func<T, TResult>> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            var (mi, args) = GetMethodInfo(func);
            return (TResult)(object)InvokeInternalAsync(mi, args, cancellationToken);
        }

        private static (MethodInfo, object[]) GetMethodInfo(LambdaExpression expr)
        {
            if (expr.Body is MethodCallExpression mce)
            {
                var args = new List<object>();
                foreach(var argument in mce.Arguments)
                {
                    var le = Expression.Lambda(argument);
                    args.Add(le.Compile().DynamicInvoke());
                }
                
                return (mce.Method, args.ToArray());
            }
            throw new Exception("expression should be a method call.");
        }
    }
}
