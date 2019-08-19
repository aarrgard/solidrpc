﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;

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
            CachedBindings = new ConcurrentDictionary<MethodInfo, IMethodBinding>();
        }

        public IOpenApiSpec OpenApiSpec { get; }

        public Assembly Assembly { get; }

        public IEnumerable<IMethodBinding> MethodBindings
        {
            get
            {
                 return CachedBindings.Values;
            }
        }

        private ConcurrentDictionary<MethodInfo, IMethodBinding> CachedBindings { get; }

        public IMethodBinding CreateMethodBinding(MethodInfo methodInfo, MethodAddressTransformer methodAddressTransformer)
        {
            return CachedBindings.GetOrAdd(methodInfo, _ => CreateBinding(_, methodAddressTransformer, true));
        }

        protected abstract IMethodBinding CreateBinding(MethodInfo mi, MethodAddressTransformer methodAddressTransformer, bool mustExist);
    }
}


