using System.IO;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Represents a response
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        public Stream ResponseStream { get; set; }
        int IHttpResponse.StatusCode { get; set; }
        string IHttpResponse.ContentType { get; set; }
    }
}
