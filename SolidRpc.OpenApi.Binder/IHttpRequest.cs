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
        string Host { get; set; }

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
        /// The data in the form
        /// </summary>
        IEnumerable<HttpRequestData> FormData { get; set; }

        /// <summary>
        /// The body
        /// </summary>
        HttpRequestData Body { get; set; }
    }
}
