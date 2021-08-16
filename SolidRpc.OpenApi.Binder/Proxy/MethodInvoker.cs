using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
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
        public const string RequestHeaderPrefixInInvocation = "http_req_";
        public const string ResponseHeaderPrefixInInvocation = "http_resp_";

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

            public IEnumerable<IMethodBinding> MethodBindings { get; set; }

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
                work.MethodBindings = (work.MethodBindings == null) ?
                    new[] { methodBinding } : work.MethodBindings.Union(new[] { methodBinding }).ToArray();
            }

            public IEnumerable<IMethodBinding> GetMethodBindings(IEnumerator<string> segments)
            {
                if (!segments.MoveNext())
                {
                    return MethodBindings;
                }
                if (!SubSegments.TryGetValue(segments.Current, out PathSegment subSegment))
                {
                    if (!SubSegments.TryGetValue("{}", out subSegment))
                    {
                        throw new Exception($"Failed to find segment {segments.Current} among segments {string.Join(",", SubSegments.Keys)}");
                    }
                }
                return subSegment.GetMethodBindings(segments);
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
                Logger.LogInformation($"Creating new root segments...");
                rootSegment = new PathSegment();
                var methodBindings = MethodBinderStore.MethodBinders
                    .SelectMany(o => o.MethodBindings)
                    .ToList();
                methodBindings.ForEach(methodBinding =>
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
                Logger.LogInformation($"...root segments created");
            }
            _rootSegment = rootSegment;
            return rootSegment;
        }

        public Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource,
            IHttpRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var pathKey = $"{request.Method}{request.Path}";
            var methodBindings = RootSegment.GetMethodBindings(pathKey.Split('/').AsEnumerable().GetEnumerator());
            if(methodBindings == null)
            {
                Logger.LogError("Could not find mapping for path " + pathKey);
                return Task.FromResult<IHttpResponse>(new SolidHttpResponse()
                {
                    StatusCode = 404
                });
            }
            return InvokeAsync(serviceProvider, invocationSource, request, methodBindings, cancellationToken);
        }

        public async Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource,
            IHttpRequest request,
            IEnumerable<IMethodBinding> methodBindings, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var invocationValues = new Dictionary<string, object>();

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation($"Method invoker handling http request:{request.Scheme}://{request.HostAndPort}{request.Path}");
            }

            if (!methodBindings?.Any() ?? true)
            {
                throw new ArgumentException("No method bindings supplied");
            }
            var selectedBinding = methodBindings.First();

            //
            // check if we should just forward the calls
            //
            var transport = selectedBinding.Transports.SingleOrDefault(o => o.GetTransportType() == invocationSource.TransportType);
            if (transport == null)
            {
                throw new Exception($"Invocation originates from {invocationSource.TransportType} but no such transport is configured ({string.Join(",", selectedBinding.Transports.Select(o => o.GetTransportType()))}).");
            }

            if (transport.InvocationStrategy == InvocationStrategy.Forward)
            {
                //
                // switch invocation source
                //
                var invokeTransport = selectedBinding.Transports.First(o => o.InvocationStrategy == InvocationStrategy.Invoke);
                if (Logger.IsEnabled(LogLevel.Trace))
                {
                    Logger.LogTrace($"Forwarding call from transport {transport.GetTransportType()} to transport {invokeTransport.GetTransportType()}");
                }
                invocationValues[typeof(InvocationOptions).FullName] = new InvocationOptions(invokeTransport.GetTransportType(), InvocationOptions.MessagePriorityNormal);
            }

            //
            // Locate service
            //
            var svc = serviceProvider.GetService(selectedBinding.MethodInfo.DeclaringType);
            if (svc == null)
            {
                throw new Exception($"Failed to resolve service for type {selectedBinding.MethodInfo.DeclaringType}");
            }
            var proxy = (ISolidProxy)svc;
            if (proxy == null)
            {
                throw new Exception($"Service for {selectedBinding.MethodInfo.DeclaringType} is not a solid proxy.");
            }

            var resp = new SolidHttpResponse();
            //
            // extract arguments
            //
            object[] args;
            try
            {
                request.Principal = serviceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal;
                args = await selectedBinding.ExtractArgumentsAsync(request);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to extract arguments - returning 400 - bad request");
                resp.StatusCode = 400;
                return resp;
            }

            //
            // set http headers
            //
            foreach (var qv in request.Headers)
            {
                var headerName = $"{RequestHeaderPrefixInInvocation }{qv.Name}".ToLower();
                if (invocationValues.TryGetValue(headerName, out object value))
                {
                    invocationValues[headerName] = StringValues.Concat((StringValues)value, qv.GetStringValue());
                }
                else
                {
                    invocationValues.Add(headerName, new StringValues(qv.GetStringValue()));
                }
            }

            try
            {
                //
                // Invoke
                //
                var res = await proxy.InvokeAsync(invocationSource, selectedBinding.MethodInfo, args, invocationValues);

                //
                // return response
                //
                // the InvokeAsync never returns a Task<T>. Strip off the task type if exists...
                //
                var resType = selectedBinding.MethodInfo.ReturnType;
                if (resType.IsTaskType(out Type taskType))
                {
                    resType = taskType ?? resType;
                }
                //
                // bind the response
                //
                await selectedBinding.BindResponseAsync(resp, res, resType);
            }
            catch (Exception ex)
            {
                // handle exception
                Logger.LogError(ex, "Service returned an exception - sending to client");
                await selectedBinding.BindResponseAsync(resp, ex, selectedBinding.MethodInfo.ReturnType);
            }

            foreach (var respHeader in invocationValues.Where(o => o.Key.StartsWith(ResponseHeaderPrefixInInvocation)))
            {
                resp.AdditionalHeaders[respHeader.Key.Substring(ResponseHeaderPrefixInInvocation.Length)] = respHeader.Value.ToString();
            }

            return resp;
        }
    }
}
