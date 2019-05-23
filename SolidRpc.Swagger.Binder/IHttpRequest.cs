using System.Collections.Generic;

namespace SolidRpc.Swagger.Binder
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
        /// The path
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The request headers
        /// </summary>
        IDictionary<string, IEnumerable<string>> Headers { get; }

        /// <summary>
        /// The request query string
        /// </summary>
        IDictionary<string, IEnumerable<string>> Query { get; }

        /// <summary>
        /// The body
        /// </summary>
        object Body { get; }
    }
}
