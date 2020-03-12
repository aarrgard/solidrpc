using Microsoft.Extensions.Logging;
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
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IMethodInvoker), typeof(MethodInvoker))]
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
                foreach(var segment in segments)
                {
                    var key = segment;
                    if(key.StartsWith("{"))
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
                        throw new Exception($"Failed to find segment {segments.Current} among segments {string.Join(",",SubSegments.Keys)}");
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
        }

        public ILogger Logger { get; }
        public IServiceProvider ServiceProvider { get; }
        public ISolidProxyConfigurationStore ProxyConfigurationStore { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        private PathSegment RootSegment
        {
            get
            {
                if(_rootSegment == null)
                {
                    lock(_mutex)
                    {
                        if(_rootSegment == null)
                        {
                            _rootSegment = new PathSegment();
                            ProxyConfigurationStore.ProxyConfigurations.ToList()
                                .SelectMany(o => o.InvocationConfigurations)
                                .Where(o => o.GetSolidInvocationAdvices().OfType<ISolidProxyInvocationAdvice>().Any())
                                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                                .ToList().ForEach(invocConfig =>
                                {
                                    var openApiConfig = invocConfig.ConfigureAdvice<ISolidRpcOpenApiConfig>();
                                    var mi = openApiConfig.InvocationConfiguration.MethodInfo;
                                    var methodInfo = MethodBinderStore.CreateMethodBinding(openApiConfig.OpenApiSpec, invocConfig.HasImplementation, mi, openApiConfig.MethodAddressTransformer);
                                    _rootSegment.AddPath(methodInfo);
                                    Logger.LogInformation($"Added {mi.DeclaringType.FullName}.{mi.Name}@{methodInfo.Address.LocalPath}.");
                                });

                        }
                    }
                }
                return _rootSegment;
            }
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
                invocationValues.Add(qv.Name, qv.GetStringValue());
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
    }
}
