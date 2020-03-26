﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Transport;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Base class for the method binders in V2 and V3
    /// </summary>
    public abstract class MethodBinderBase : IMethodBinder
    {

        protected MethodBinderBase(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            OpenApiSpec = openApiSpec;
            Assembly = assembly;
            CachedBindings = new ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>>();
        }

        public IOpenApiSpec OpenApiSpec { get; }

        public Assembly Assembly { get; }

        public IEnumerable<IMethodBinding> MethodBindings
        {
            get
            {
                return CachedBindings.Values.SelectMany(o => o);
            }
        }

        private ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>> CachedBindings { get; }

        public IEnumerable<IMethodBinding> CreateMethodBindings(
            MethodInfo methodInfo,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey)
        {
            return CachedBindings.GetOrAdd(methodInfo, _ => CreateBindings(_, transports, securityKey));
        }

        private IEnumerable<IMethodBinding> CreateBindings(
            MethodInfo mi,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey)
        {
            var methodBindings = DoCreateMethodBinding(mi, transports, securityKey);

            // sort em according to the preferred method
            methodBindings = methodBindings.OrderBy(o => GetMethodIdx(o.Method));

            // Configure transports
            methodBindings.ToList().ForEach(methodBinding =>
            {
                transports.ToList().ForEach(o => o.Configure(methodBinding));
            });

            return methodBindings;
        }

        private int GetMethodIdx(string method)
        {
            switch(method.ToLower())
            {
                case "get": return 1;
                case "options": return 2;
                case "put": return 3;
                case "delete": return 4;
                default: return 100;
            }
        }

        protected abstract IEnumerable<IMethodBinding> DoCreateMethodBinding(
            MethodInfo mi,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey);
    }
}

