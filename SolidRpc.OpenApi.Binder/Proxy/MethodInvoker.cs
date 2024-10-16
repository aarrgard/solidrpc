﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.InternalServices;
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
        public const string RequestHeaderContinuationTokenInInvocation = "http_req_X-SolidRpc-ContinuationToken";
        public const string RequestHeaderMethodUri = "http_req_X-SolidRpc-MethodUri";
        public const string ResponseHeaderPrefixInInvocation = "http_resp_";

        private class PathSegment
        {
            private static readonly IEnumerable<IMethodBinding> EmptyMethodBindings = new IMethodBinding[0];

            public PathSegment()
            {
                SubSegments = new Dictionary<string, PathSegment>();
                MethodBindings = EmptyMethodBindings;
            }
            public PathSegment(PathSegment parentSegment) : this()
            {
                ParentSegment = parentSegment;
                MethodBindings = EmptyMethodBindings;
            }

            public PathSegment ParentSegment { get; }

            private IDictionary<string, PathSegment> SubSegments { get; }

            public IEnumerable<IMethodBinding> MethodBindings { get; set; }

            public Func<IHttpRequest, IHttpRequest> Rewrite { get; set; }

            public void AddPath(IMethodBinding methodBinding)
            {
                AddPath(methodBinding.Method, methodBinding.LocalPath, r => r, methodBinding);
                AddPath("OPTIONS", methodBinding.LocalPath, r => r, methodBinding);
            }

            public void AddPath(string method, string path, Func<IHttpRequest, IHttpRequest> rewrite, IMethodBinding methodBinding)
            {
                var work = this;
                var segments = $"{method}{path}".Split('/');
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
                work.MethodBindings = work.MethodBindings.Union(new[] { methodBinding }).ToArray();
                work.Rewrite = rewrite;
            }

            public PathSegment GetPathSegment(IEnumerator<string> segments)
            {
                if (!segments.MoveNext())
                {
                    return this;
                }
                if (!SubSegments.TryGetValue(segments.Current, out PathSegment subSegment))
                {
                    if (!SubSegments.TryGetValue("{}", out subSegment))
                    {
                        return null;
                        //throw new Exception($"Failed to find segment {segments.Current} among segments {string.Join(",", SubSegments.Keys)}");
                    }
                }
                return subSegment.GetPathSegment(segments);
            }
        }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private PathSegment _rootSegment;

        public MethodInvoker(
            ILogger<MethodInvoker> logger,
            IMethodAddressTransformer methodAddressTransformer,
            ISolidRpcContentHandler contentHandler,
            IMethodBinderStore methodBinderStore)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            MethodInfo2Binding = new Dictionary<MethodInfo, IMethodBinding>();
            MethodAddressTransformer = methodAddressTransformer;
            ContentHandler = contentHandler;
        }

        private ILogger Logger { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        private Dictionary<MethodInfo, IMethodBinding>  MethodInfo2Binding { get; }
        private IMethodAddressTransformer MethodAddressTransformer { get; }
        private ISolidRpcContentHandler ContentHandler { get; }

        private async Task<PathSegment> GetRootSegmentAsync(CancellationToken cancellationToken)
        {
            var rootSegment = _rootSegment;
            if (_rootSegment != null)
            {
                return _rootSegment;
            }
            await _semaphore.WaitAsync(cancellationToken);
            try 
            {
                Logger.LogInformation($" new root segments...");
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

                // Add prefix mappings into content handler
                var prefixes1 = (await ContentHandler.GetPathMappingsAsync(true)).Select(o => o.Name);
                var prefixes2 = (await ContentHandler.GetPathMappingsAsync(false)).Select(o => o.Name);
                var prefixes3 = ContentHandler.PathPrefixes;
                var contentBinding = MethodBinderStore.GetMethodBinding<ISolidRpcContentHandler>(o => o.GetContent("/", cancellationToken));
                prefixes1.Union(prefixes2).Union(prefixes3).ToList().ForEach(o =>
                {
                    var prefix = MethodAddressTransformer.RewritePath(o);          
                    if(prefix.EndsWith("*"))
                    {
                        prefix = prefix.Substring(0, prefix.Length-1);
                    }
                    if (!prefix.EndsWith("/"))
                    {
                        prefix = $"{prefix}/";
                    }
                    prefix = $"{prefix}{{}}";
                    Func<IHttpRequest, IHttpRequest> rewrite = (r) =>
                    {
                        r.Query = new[] { new SolidHttpRequestDataString("text/plain", "path", r.Path) };
                        r.Path = contentBinding.LocalPath;
                        return r;
                    };
                    rootSegment.AddPath("GET", prefix, rewrite, contentBinding);
                    rootSegment.AddPath("OPTIONS", prefix, rewrite, contentBinding);
                });

                Logger.LogInformation($"...root segments created");
            } 
            finally
            {
                _semaphore.Release();
            }
            _rootSegment = rootSegment;
            return rootSegment;
        }

        public async Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource,
            IHttpRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var pathKey = $"{request.Method}{request.Path}";
            var pathSegment = (await GetRootSegmentAsync(cancellationToken)).GetPathSegment(pathKey.Split('/').AsEnumerable().GetEnumerator());
            if(pathSegment == null || !pathSegment.MethodBindings.Any())
            {
                Logger.LogError("Could not find mapping for path " + pathKey);
                return new SolidHttpResponse()
                {
                    StatusCode = 404
                };
            }
            request = pathSegment.Rewrite(request);
            return await InvokeAsync(serviceProvider, invocationSource, request, pathSegment.MethodBindings, cancellationToken);
        }

        public async Task<IHttpResponse> InvokeAsync(
            IServiceProvider serviceProvider,
            ITransportHandler invocationSource,
            IHttpRequest request,
            IEnumerable<IMethodBinding> methodBindings, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var resp = new SolidHttpResponse();

            //
            // handle the access-control(CORS) request for this invocation
            //
            var allowedCorsOrigins = MethodAddressTransformer.AllowedCorsOrigins;
            if (!request.CheckCorsIsValid(allowedCorsOrigins, out string origin))
            {
                // request not allowed
                resp.StatusCode = 401;
                serviceProvider.LogInformation<MethodInvoker>($"Rejecting request. {origin} not part of allowed origins {string.Join(",", allowedCorsOrigins)}");
                return resp;
            }

            //
            // handle cors invocation
            //
            if (string.Equals(request.Method, "options", StringComparison.InvariantCultureIgnoreCase))
            {
                resp.StatusCode = 204;
                resp.AddAllowedCorsHeaders(request);
                return resp;
            }

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

            var invokerTransport = transport.InvokerTransport;
            if (!string.IsNullOrEmpty(invokerTransport))
            {
                //
                // switch invocation source
                //
                var invokeTransport = selectedBinding.Transports.First(o => o.GetTransportType() == invokerTransport);
                if(invokerTransport == null)
                {
                    throw new Exception($"The transport {transport.GetTransportType()} cannot forward to transport {invokeTransport.GetTransportType()} since it is not configured");

                }
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

            //
            // extract arguments
            //
            object[] args;
            try
            {
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
                var headerName = $"{RequestHeaderPrefixInInvocation}{qv.Name}".ToLower();
                if (invocationValues.TryGetValue(headerName, out object value))
                {
                    invocationValues[headerName] = StringValues.Concat((StringValues)value, qv.GetStringValue());
                }
                else
                {
                    invocationValues.Add(headerName, new StringValues(qv.GetStringValue()));
                }
            }

            //
            // recreate uri for redirects
            //
            invocationValues.Add(RequestHeaderMethodUri, selectedBinding.BindUri(request, transport.OperationAddress));

            //
            // set continuation token
            //
            var continuationToken = serviceProvider.GetRequiredService<ISolidRpcContinuationToken>();
            if(invocationValues.TryGetValue($"{RequestHeaderPrefixInInvocation}{continuationToken.GetHttpHeaderName()}".ToLower(), out object token)) 
            {
                continuationToken.Token = token?.ToString();
            }
            using (continuationToken.PushToken())
            {
                try
                {
                    //
                    // Invoke
                    //
                    var res = await proxy.InvokeAsync(serviceProvider, invocationSource, selectedBinding.MethodInfo, args, invocationValues);

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
                    // bind the response & continuation token
                    //
                    await selectedBinding.BindResponseAsync(resp, res, resType);
                    if(!string.IsNullOrEmpty(continuationToken.Token))
                    {
                        resp.AdditionalHeaders.Add(continuationToken.GetHttpHeaderName(), continuationToken.Token);
                    }
                }
                catch (Exception ex)
                {
                    // Only log error if this is a non http service code...
                    var httpStatusCode = ex.Data["HttpStatusCode"];
                    if (httpStatusCode == null)
                    {
                        Logger.LogError(ex, "Service returned an exception - sending to client");
                    }
                    else 
                    {
                        Logger.LogInformation($"Service returned an exception with http status {httpStatusCode}:{ex.Message}");
                    }
                    await selectedBinding.BindResponseAsync(resp, ex, selectedBinding.MethodInfo.ReturnType);
                }

                foreach (var respHeader in invocationValues.Where(o => o.Key.StartsWith(ResponseHeaderPrefixInInvocation)))
                {
                    resp.AdditionalHeaders[respHeader.Key.Substring(ResponseHeaderPrefixInInvocation.Length)] = respHeader.Value.ToString();
                }
            }

            resp.AddAllowedCorsHeaders(request);

            return resp;
        }
    }
}
