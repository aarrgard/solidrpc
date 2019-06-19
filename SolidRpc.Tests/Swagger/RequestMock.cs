﻿using System.Collections.Generic;
using SolidRpc.OpenApi.Binder;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestMock : IHttpRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public RequestMock()
        {
            Headers = new HttpRequestData[0];
            Query = new HttpRequestData[0];
            BodyData = new HttpRequestData[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HostAndPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<HttpRequestData> Headers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<HttpRequestData> Query { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<HttpRequestData> BodyData { get; set; }
    }
}
