using System.Collections.Generic;
using SolidRpc.Swagger.Binder;

namespace SolidRpc.Tests.Swagger
{
    public class RequestMock : IHttpRequest
    {
        public RequestMock()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
            Query = new Dictionary<string, IEnumerable<string>>();
        }
 
        public string Path { get; set; }

        public string Method { get; set; }

        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public IDictionary<string, IEnumerable<string>> Query { get; }

        public object Body => null;
    }
}
