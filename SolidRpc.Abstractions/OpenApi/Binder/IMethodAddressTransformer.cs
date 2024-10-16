﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Interface that can be implemented to resolve the base uri.
    /// </summary>
    public interface IMethodAddressTransformer
    {
        /// <summary>
        /// Returns the allowed origins
        /// </summary>
        IEnumerable<string> AllowedCorsOrigins { get; }

        /// <summary>
        /// Returns the base address for this server
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// Rewrites the supplied path based on the rewrite rules
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string RewritePath(string path);

        /// <summary>
        /// Returns the uri for supplied method. If no method is supplied
        /// the base address for the open api spec is determined.
        /// </summary>
        Uri TransformUri(Uri uri, MethodInfo methodInfo = null);
    }
}
