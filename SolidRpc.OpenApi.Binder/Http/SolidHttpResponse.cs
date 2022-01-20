using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents a response
    /// </summary>
    public class SolidHttpResponse : IHttpResponse
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidHttpResponse()
        {
            AdditionalHeaders = new Dictionary<string, string>(0);
        }

        /// <summary>
        /// The response stream
        /// </summary>
        public Stream ResponseStream { get; set; }

        /// <summary>
        /// The status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// The char set
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The last modified time
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// The location of the resource
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The ETag.
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// The ETag.
        /// </summary>
        public string SetCookie { get; set; }

        /// <summary>
        /// The additional headers
        /// </summary>
        public IDictionary<string, string> AdditionalHeaders { get; }
    }
}
