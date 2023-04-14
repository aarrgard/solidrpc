using SolidRpc.Abstractions.OpenApi.Http;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<IHttpRequestData> PathData { get; set; }

        /// <summary>
        /// The headers
        /// </summary>
        public IEnumerable<IHttpRequestData> Headers { get; set; }

        /// <summary>
        /// Sets the header value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetHeader(string name, string value)
        {
            Headers = Headers
                .Where(o => !string.Equals(name, o.Name, System.StringComparison.InvariantCultureIgnoreCase))
                .Union(new[] { new SolidHttpRequestDataString("text/plain", name, value) })
                .ToList();
        }

        /// <summary>
        /// The query data
        /// </summary>
        public IEnumerable<IHttpRequestData> Query { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Contains the data in the body.
        /// </summary>
        public IEnumerable<IHttpRequestData> BodyData { get; set; }
    }
}
