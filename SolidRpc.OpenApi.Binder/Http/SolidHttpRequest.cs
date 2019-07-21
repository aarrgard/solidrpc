using System.Collections.Generic;
using System.Threading;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Implementation of the IHttpRequest object
    /// </summary>
    public class SolidHttpRequest : IHttpRequest
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidHttpRequest()
        {
            CancellationToken = CancellationToken.None;
            PathData = SolidHttpRequestData.EmptyArray;
            Headers = SolidHttpRequestData.EmptyArray;
            Query = SolidHttpRequestData.EmptyArray;
            BodyData = SolidHttpRequestData.EmptyArray;
            Scheme = "http";
        }

        /// <summary>
        /// The cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

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
        /// The path data
        /// </summary>
        public IEnumerable<SolidHttpRequestData> PathData { get; set; }

        /// <summary>
        /// The headers
        /// </summary>
        public IEnumerable<SolidHttpRequestData> Headers { get; set; }

        /// <summary>
        /// The query data
        /// </summary>
        public IEnumerable<SolidHttpRequestData> Query { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Contains the data in the body.
        /// </summary>
        public IEnumerable<SolidHttpRequestData> BodyData { get; set; }
    }
}
