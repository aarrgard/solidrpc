using SolidRpc.Abstractions.OpenApi.Http;
using System.IO;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents a response
    /// </summary>
    public class SolidHttpResponse : IHttpResponse
    {
        public Stream ResponseStream { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
    }
}
