using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a HttpRequest. We can use this type to create dynamic invocation
    /// handlers that intercepts all the data sent to it.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// The method(GET,POST,PUT,etc)
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The uri
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The headers
        /// </summary>
        public IDictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// The body.
        /// </summary>
        public Stream Body { get; set; }
    }
}
