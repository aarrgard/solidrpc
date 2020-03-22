using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

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
        public IDictionary<string, StringValues> Headers { get; set; }

        /// <summary>
        /// The query parameters
        /// </summary>
        public IDictionary<string, StringValues> Query { get; set; }

        /// <summary>
        /// The body.
        /// </summary>
        public FileContent Body { get; set; }
    }
}
