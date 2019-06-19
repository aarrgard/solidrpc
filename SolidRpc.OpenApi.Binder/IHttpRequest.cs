using System.Collections.Generic;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Interface that we use to access the data in the Http request
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// The method to use
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// The scheme to use.
        /// </summary>
        string Scheme { get; set; }

        /// <summary>
        /// The host to use.
        /// </summary>
        string HostAndPort { get; set; }

        /// <summary>
        /// The path
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The request headers
        /// </summary>
        IEnumerable<HttpRequestData> Headers { get; set; }

        /// <summary>
        /// The request query string
        /// </summary>
        IEnumerable<HttpRequestData> Query { get; set; }

        /// <summary>
        /// The content type of the body data.
        /// 
        /// When composing a request a call that consumes 
        ///  - "application/x-www-form-urlencoded"
        ///  - "multipart/form-data"
        /// will create the request accordingly.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// The data in the body. Check the ContentType to 
        /// determine how the data should be transmitted.
        /// </summary>
        IEnumerable<HttpRequestData> BodyData { get; set; }
    }
}
