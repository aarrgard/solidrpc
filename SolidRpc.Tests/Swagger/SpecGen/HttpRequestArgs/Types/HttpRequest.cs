using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// The method used
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Uri
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
