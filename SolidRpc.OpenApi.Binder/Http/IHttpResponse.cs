using System.IO;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Http
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
        /// Returns the content type
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Returns the response stream
        /// </summary>
        /// <returns></returns>
        Stream ResponseStream { get; set; }
    }
}
