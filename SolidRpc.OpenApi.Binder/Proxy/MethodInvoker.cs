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

            public IMethodInfo MethodInfo { get; set; }

            public void AddPath(IMethodInfo methodInfo)
            {
                var work = this;
                var segments = $"{methodInfo.Method}{methodInfo.Path}".Split('/');
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

            public IMethodInfo GetMethodInfo(IEnumerable<string> segments)
            {
                if (!segments.Any())
                {
                    return MethodInfo;
                }
                if (!SubSegments.TryGetValue(segments.First(), out PathSegment subSegment))
                {
                    if (!SubSegments.TryGetValue("{}", out subSegment))
                    {
                        return null;
                    }
                }
                var rest = segments.Skip(1);
                return subSegment.GetMethodInfo(rest);
            }
        }

        private object _mutex = new object();
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
                            ProxyConfigurationStore.ProxyConfigurations
                                .SelectMany(o => o.InvocationConfigurations)
                                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                                .Select(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>())
                                .ToList().ForEach(o =>
                                {
                                    var mi = o.InvocationConfiguration.MethodInfo;
                                    var methodInfo = MethodBinderStore.GetMethodInfo(o.GetOpenApiConfiguration(), mi);
                                    _rootSegment.AddPath(methodInfo);
                                    Logger.LogInformation($"Added {mi.DeclaringType.FullName}.{mi.Name}@{methodInfo.Path}.");
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
            var methodInfo = RootSegment.GetMethodInfo(pathKey.Split('/'));
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
            IMethodInfo methodInfo, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
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

            // return response
            var resp = new SolidHttpResponse();
            try
            {
                var res = await proxy.InvokeAsync(methodInfo.MethodInfo, args);

                await methodInfo.BindResponseAsync(resp, res, methodInfo.MethodInfo.ReturnType);
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
