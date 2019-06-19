﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implementation of the IHttpRequest object
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public HttpRequest()
        {
            Headers = HttpRequestData.EmptyArray;
            Query = HttpRequestData.EmptyArray;
            BodyData = HttpRequestData.EmptyArray;
            Scheme = "http";
        }

        /// <summary>
        /// The method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The scheme
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// The host
        /// </summary>
        public string HostAndPort { get; set; }

        /// <summary>
        /// The path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The headers
        /// </summary>
        public IEnumerable<HttpRequestData> Headers { get; set; }

        /// <summary>
        /// The query data
        /// </summary>
        public IEnumerable<HttpRequestData> Query { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Contains the data in the body.
        /// </summary>
        public IEnumerable<HttpRequestData> BodyData { get; set; }
    }
}
