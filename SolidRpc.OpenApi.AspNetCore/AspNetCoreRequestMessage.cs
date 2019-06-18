using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SolidRpc.OpenApi.Binder;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Represents a request
    /// </summary>
    public class AspNetCoreRequestMessage : IHttpRequest
    {
        public AspNetCoreRequestMessage(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; }

        public string Method { get => Request.Method; set => Request.Method = value; }
        public string Scheme { get => Request.Scheme; set => Request.Scheme = value; }
        public string Host { get => Request.Host.ToString(); set => Request.Host = new HostString(value); }
        public string Path { get => Request.Path; set => Request.Path = value; }
        public IEnumerable<HttpRequestData> Headers { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IEnumerable<HttpRequestData> Query { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IEnumerable<HttpRequestData> FormData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public HttpRequestData Body { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}