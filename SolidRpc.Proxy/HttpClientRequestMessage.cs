using Newtonsoft.Json;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace SolidRpc.Proxy
{
    /// <summary>
    /// Implements the logic that the binder uses to bind request data.
    /// </summary>
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

                FormUrlEncodedContent formContent = null;
                if(FormData.Any())
                {
                    var data = FormData.Select(o => new KeyValuePair<string, string>(o.Name, o.GetStringValue()));
                    formContent = new FormUrlEncodedContent(data);
                }

                MultipartFormDataContent formData = null;
                if (Body != null)
                {
                    HttpContent content;
                    switch(Body.ContentType.ToLower())
                    {
                        case "application/octet-stream":
                            content = new StreamContent(Body.GetBinaryValue());
                            content.Headers.ContentType = new MediaTypeHeaderValue(Body.ContentType);
                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = Body.Name, FileName = "test.xml" };

                            formData = new MultipartFormDataContent();
                            if (formContent != null)
                            {
                                formData.Add(formContent);
                            }
                            break;
                        case "application/json":
                            var enc = Encoding.UTF8;
                            content = new StringContent(Body.GetStringValue(enc));
                            content.Headers.ContentType = new MediaTypeHeaderValue(Body.ContentType) { CharSet = enc.HeaderName };
                            break;
                        default:
                            throw new Exception("Cannot handle content type:" + Body.ContentType);
                    }
                    if(formData != null)
                    {
                        formData.Add(content);
                    }
                    else
                    {
                        req.Content = content;
                    }
                }
                if(formData != null)
                {
                    req.Content = formData;
                }
                else if(formContent != null)
                {
                    req.Content = formContent;
                }
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
