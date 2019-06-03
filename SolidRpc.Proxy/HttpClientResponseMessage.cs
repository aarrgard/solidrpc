using SolidRpc.Swagger.Binder;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.Proxy
{
    public class HttpClientResponseMessage : IHttpResponse
    {
        public HttpClientResponseMessage(HttpResponseMessage httpClientResponse)
        {
            HttpClientResponse = httpClientResponse;
        }

        public HttpResponseMessage HttpClientResponse { get; }

        public string ContentType => HttpClientResponse.Content.Headers.ContentType.MediaType;

        public int StatusCode => (int)HttpClientResponse.StatusCode;

        public Task<Stream> GetResponseStreamAsync()
        {
            return HttpClientResponse.Content.ReadAsStreamAsync();
        }
    }
}
