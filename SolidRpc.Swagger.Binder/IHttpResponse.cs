using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Interface that we use to access the data in the Http response
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// Returns the content type
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Returns the response stream
        /// </summary>
        /// <returns></returns>
        Task<Stream> GetResponseStreamAsync();
    }
}
