using System;
using System.Collections.Generic;
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
        /// Returns the media type - stored in the Content-Type header.
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// Returns the char et - stored in the Content-Type header.
        /// </summary>
        string CharSet { get; set; }

        /// <summary>
        /// The filename - stored in Content-Disposition header
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// The location - stored in Location header
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// The ETag - stored in Location header
        /// </summary>
        string ETag { get; set; }

        /// <summary>
        /// Accessor for the "Last-Modified" header.
        /// </summary>
        DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// Accessor for the "Set-Cookie" header.
        /// </summary>
        string SetCookie { get; set; }

        /// <summary>
        /// Returns the response stream
        /// </summary>
        /// <returns></returns>
        Stream ResponseStream { get; set; }

        /// <summary>
        /// The additional headers
        /// </summary>
        IDictionary<string, string> AdditionalHeaders { get; }
    }
}
