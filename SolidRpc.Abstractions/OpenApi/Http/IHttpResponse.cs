using System;
using System.IO;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Interface that we use to access the data in the Http response
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// Returns the status code
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Returns the content type - stored in the Content-Type header.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// The filename - stored in Content-Disposition header
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// Accessor for the "Last-Modified" header.
        /// </summary>
        DateTime? LastModified { get; set; }

        /// <summary>
        /// Returns the response stream
        /// </summary>
        /// <returns></returns>
        Stream ResponseStream { get; set; }
    }
}
