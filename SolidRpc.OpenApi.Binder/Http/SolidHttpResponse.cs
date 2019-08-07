using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.IO;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents a response
    /// </summary>
    public class SolidHttpResponse : IHttpResponse
    {
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
        public string ContentType { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The last modified time
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
