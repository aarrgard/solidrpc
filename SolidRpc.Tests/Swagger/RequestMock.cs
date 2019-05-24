using System.Collections.Generic;
using SolidRpc.Swagger.Binder;

namespace SolidRpc.Tests.Swagger
{
    public class RequestMock : IHttpRequest
    {
        public RequestMock()
        {
            Headers = new HttpRequestData[0];
            Query = new HttpRequestData[0];
            FormData = new HttpRequestData[0];
        }

        public string Path { get; set; }

        public string Method { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }

        public IEnumerable<HttpRequestData> Headers { get; set; }

        public IEnumerable<HttpRequestData> Query { get; set; }

        public IEnumerable<HttpRequestData> FormData { get; set; }

        public object Body { get; set; }
    }
}
