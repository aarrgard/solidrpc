using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a HttpRequest. We can use this type to create dynamic invocation
    /// handlers that intercepts all the data sent to it.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// The method(GET,POST,PUT,etc)
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The uri
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The headers
        /// </summary>
        public IDictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// The body.
        /// </summary>
        public Stream Body { get; set; }
    }

    /// <summary>
    /// The request extensions
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Sets the header value
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="headerName"></param>
        /// <param name="headerValue"></param>
        /// <returns></returns>
        public static HttpRequest SetHeader(this HttpRequest httpRequest, string headerName, string headerValue)
        {
            httpRequest.Headers[headerName] = new[] { headerValue };
            return httpRequest;
        }

        /// <summary>
        /// Returns the header value
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static string GetHeader(this HttpRequest httpRequest, string headerName)
        {
            if (!httpRequest.Headers.TryGetValue(headerName, out string[] value))
            {
                return null;
            }
            return value?.FirstOrDefault();
        }

        /// <summary>
        /// Sets the solid rpc call id
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="callId"></param>
        /// <returns></returns>
        public static HttpRequest SetSolidRpcCallId(this HttpRequest httpRequest, string callId)
        {
            httpRequest.SetHeader("X-SolidRpc-CallId", callId);
            return httpRequest;
        }
        /// <summary>
        /// Sets the solid rpc call id
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static string GetSolidRpcCallId(this HttpRequest httpRequest)
        {
            return httpRequest.GetHeader("X-SolidRpc-CallId");
        }
    }
}
