using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcService(typeof(IMethodInvoker), typeof(MethodInvoker))]
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

            public void AddPath(IMethodBinding methodBinding)
            {
                var work = this;
                var segments = $"{methodBinding.Method}{methodBinding.LocalPath}".Split('/');
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
                work.MethodInfo = methodBinding;
            }

            public IMethodBinding GetMethodBinding(IEnumerator<string> segments)
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
                return subSegment.GetMethodBinding(segments);
            }
        }

        private readonly object _mutex = new object();
        private PathSegment _rootSegment;

        public MethodInvoker(
            ILogger<MethodInvoker> logger,
            IMethodBinderStore methodBinderStore)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            MethodInfo2Binding = new Dictionary<MethodInfo, IMethodBinding>();
        }

        public ILogger Logger { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        public Dictionary<MethodInfo, IMethodBinding>  MethodInfo2Binding { get; }
        private PathSegment RootSegment => GetRootSegment();

        private PathSegment GetRootSegment()
        {
            var rootSegment = _rootSegment;
            if (rootSegment != null)
            {
                return rootSegment;
            }
            lock(_mutex)
            {
                rootSegment = new PathSegment();
                MethodBinderStore.MethodBinders
                    .SelectMany(o => o.MethodBindings)
                    .ToList().ForEach(methodBinding =>
                {
                    rootSegment.AddPath(methodBinding);
                    var mi = methodBinding.MethodInfo;
                    Logger.LogInformation($"Added {mi.DeclaringType.FullName}.{mi.Name}@{methodBinding.LocalPath}.");

                    // we may have several bindings to the same method - choose the first one.
                    if (!MethodInfo2Binding.ContainsKey(mi))
                    {
                        MethodInfo2Binding[mi] = methodBinding;
                    }
                });

            }
            _rootSegment = rootSegment;
            return rootSegment;
        }

        public Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            IHttpRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var pathKey = $"{request.Method}{request.Path}";
            var methodInfo = RootSegment.GetMethodBinding(pathKey.Split('/').AsEnumerable().GetEnumerator());
            if(methodInfo == null)
            {
                Logger.LogError("Could not find mapping for path " + pathKey);
                return Task.FromResult<IHttpResponse>(new SolidHttpResponse()
                {
                    StatusCode = 404
                });
            }
            return InvokeAsync(serviceProvider, request, methodInfo, cancellationToken);
        }

        public async Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            IHttpRequest request, 
            IMethodBinding methodInfo, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation($"Method invoker handling http request:{request.Scheme}://{request.HostAndPort}{request.Path}");
            }

            //
            // Locate service
            //
            var svc = serviceProvider.GetService(methodInfo.MethodInfo.DeclaringType);
            if (svc == null)
            {
                throw new Exception($"Failed to resolve service for type {methodInfo.MethodInfo.DeclaringType}");
            }
            var proxy = (ISolidProxy)svc;
            if (proxy == null)
            {
                throw new Exception($"Service for {methodInfo.MethodInfo.DeclaringType} is not a solid proxy.");
            }

            //
            // extract arguments
            //
            var args = await methodInfo.ExtractArgumentsAsync(request);
            var invocationValues = new Dictionary<string, object>();
            foreach(var qv in request.Headers)
            {
                var headerName = qv.Name.ToLower();
                if(invocationValues.TryGetValue(headerName, out object value))
                {
                    invocationValues[headerName] = StringValues.Concat((StringValues)value, qv.GetStringValue());
                }
                else
                {
                    invocationValues.Add(headerName, new StringValues(qv.GetStringValue()));
                }
            }

            var resp = new SolidHttpResponse();
            try
            {
                //
                // Invoke
                //
                var res = await proxy.InvokeAsync(methodInfo.MethodInfo, args, invocationValues);

                //
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
    }
}
