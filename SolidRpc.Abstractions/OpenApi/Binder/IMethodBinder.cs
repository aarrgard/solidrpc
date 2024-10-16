﻿using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Transport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// The method binder is responsible for binding MethodInfo structures to a swagger spec.
    /// </summary>
    public interface IMethodBinder
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The open api spec that this binder gets its information from
        /// </summary>
        IOpenApiSpec OpenApiSpec { get; }

        /// <summary>
        /// The associated assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// This is the base address for this assembly. It has
        /// been transformed by the registered UrlTransformer to 
        /// match the current hosting setting.
        /// </summary>
        Uri HostedAddress { get; }

        /// <summary>
        /// Returns all the mapped methods
        /// </summary>
        IEnumerable<IMethodBinding> MethodBindings { get; }

        /// <summary>
        /// Returns the method info from supplied specification.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="transports"></param>
        /// <returns></returns>
        IEnumerable<IMethodBinding> CreateMethodBindings(
            MethodInfo methodInfo,
            IEnumerable<ITransport> transports);
    }
}
