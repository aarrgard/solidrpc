﻿using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            RootSegment = new PathSegment();

            ProxyConfigurationStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations)
                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                .Select(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>())
                .ToList().ForEach(o =>
            {
                var mi = o.InvocationConfiguration.MethodInfo;
                var methodInfo = MethodBinderStore.GetMethodInfo(o.OpenApiConfiguration, mi);
                RootSegment.AddPath(methodInfo);
                Logger.LogInformation($"Added {mi.DeclaringType.FullName}.{mi.Name}@{methodInfo.Path}.");
            });
        }

        public ILogger Logger { get; }
        public IServiceProvider ServiceProvider { get; }
        public ISolidProxyConfigurationStore ProxyConfigurationStore { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        private PathSegment RootSegment { get; }

        public Task<IHttpResponse> InvokeAsync(
            IHttpRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var pathKey = $"{request.Method}{request.Path}";
            var methodInfo = RootSegment.GetMethodInfo(pathKey.Split('/'));
            if(methodInfo == null)
            {
                Logger.LogError("Could not find mapping for path " + pathKey);
                return Task.FromResult<IHttpResponse>(new HttpResponse()
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
            var proxy = (ISolidProxy)ServiceProvider.GetService(methodInfo.MethodInfo.DeclaringType);
            if (proxy == null)
            {
                throw new Exception($"Failed to resolve proxy for type {methodInfo.MethodInfo.DeclaringType}");
            }

            // return response
            var resp = new SolidRpc.OpenApi.Binder.Http.HttpResponse();
            try
            {
                var res = await proxy.InvokeAsync(methodInfo.MethodInfo, args);

                await methodInfo.BindResponseAsync(resp, res, methodInfo.MethodInfo.ReturnType);
            }
            catch (Exception ex)
            {
                // handle exception
                Logger.LogError(ex, "Service returned an excpetion - sending to client");
                await methodInfo.BindResponseAsync(resp, ex, methodInfo.MethodInfo.ReturnType);
            }
            return resp;

        }
    }
}
