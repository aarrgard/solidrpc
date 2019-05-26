using SolidRpc.Swagger.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SolidRpc.Proxy
{
    public class HttpClientRequestMessage : IHttpRequest
    {
        private string _host;

        public HttpClientRequestMessage()
        {
            Headers = HttpRequestData.EmptyArray;
            Query = HttpRequestData.EmptyArray;
            FormData = HttpRequestData.EmptyArray;
            Port = 80;
            Scheme = "http";
        }
        public HttpRequestMessage HttpRequestMessage {
            get
            {
                var builder = new UriBuilder
                {
                    Scheme = Scheme,
                    Host = Host,
                    Path = Path,
                    Query = string.Join("&", Query.Select(o => $"{o.Name}={HttpUtility.UrlEncode(o.GetStringValue())}"))
                };
                if(Port != null)
                {
                    builder.Port = Port.Value;
                }

                var req = new HttpRequestMessage
                {
                    Method = new HttpMethod(Method),
                    RequestUri = builder.Uri
                };
                return req;
            }
        }

        public string Method { get; set; }
        public string Scheme { get; set; }
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
                if(_host != null)
                {
                    var parts = _host.Split(':');
                    if(parts.Length > 1)
                    {
                        _host = parts[0];
                        Port = int.Parse(parts[1]);
                    }
                    else
                    {
                        Port = null;
                    }
                }
            }
        }
        public int? Port { get; set; }

        public string Path { get; set; }

        public IEnumerable<HttpRequestData> Headers { get; set; }
        public IEnumerable<HttpRequestData> Query { get; set; }
        public IEnumerable<HttpRequestData> FormData { get; set; }
        public HttpRequestData Body { get; set; }
    }
}
