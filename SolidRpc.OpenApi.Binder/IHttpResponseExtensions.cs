using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IHttpResponseExtensions
    {
        private static Regex ETagRegex = new Regex("\"(.*)\"");

        /// <summary>
        /// Adds the allowed cors headers
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        public static void AddAllowedCorsHeaders(this IHttpResponse response, IHttpRequest request)
        {
            var origin = request.Headers.Where(o => o.Name == "origin").Select(o => o.GetStringValue()).FirstOrDefault();
            if (string.IsNullOrEmpty(origin))
            {
                return;
            }
            if (!string.IsNullOrEmpty(origin))
            {
                response.AdditionalHeaders.Add("Access-Control-Allow-Origin", origin);
            }
            var accessControlRequestHeaders = request.Headers.Where(o => o.Name == "Access-Control-Request-Headers").Select(o => o.GetStringValue()).FirstOrDefault();
            if (!string.IsNullOrEmpty(accessControlRequestHeaders))
            {
                response.AdditionalHeaders.Add("Access-Control-Allow-Headers", accessControlRequestHeaders);
            }
            var accessControlRequestMethod = request.Headers.Where(o => o.Name == "Access-Control-Request-Method").Select(o => o.GetStringValue()).FirstOrDefault();
            if (!string.IsNullOrEmpty(accessControlRequestMethod))
            {
                response.AdditionalHeaders.Add("Access-Control-Allow-Method", accessControlRequestMethod);
            }
            response.AdditionalHeaders.Add("Access-Control-Max-Age", "86400");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static async Task CopyFromAsync(this IHttpResponse target, HttpResponseMessage source)
        {
            target.StatusCode = (int)source.StatusCode;
            target.MediaType = source.Content?.Headers?.ContentType?.MediaType;
            if (!string.IsNullOrEmpty(target.MediaType))
            {
                target.MediaType = source.Content.Headers?.ContentType?.MediaType;
                target.CharSet = RemoveQuotes(source.Content.Headers?.ContentType?.CharSet);
                target.Filename = RemoveQuotes(source.Content.Headers?.ContentDisposition?.FileName);
                target.LastModified = source.Content.Headers?.LastModified;
                //if(source.Headers.ETag != null) // this will only be set if etag i within quotes - azure functions seems to remove them 
                if(source.Headers.TryGetValues("ETag", out IEnumerable<string> etags))
                {
                    target.ETag = RemoveQuotes(string.Join(" ", etags));
                }
                var ms = new MemoryStream();
                await source.Content.CopyToAsync(ms);
                target.ResponseStream = new MemoryStream(ms.ToArray());
            }
            if (source.Headers.Location != null)
            {
                target.Location = source.Headers.Location.ToString();
            }
            foreach(var h in source.Headers)
            {
                switch(h.Key.ToLower())
                {
                    case "location":
                        break;
                    default:
                        target.AdditionalHeaders[h.Key] = string.Join("", h.Value);
                        break;
                }
            }
        }

        private static string RemoveQuotes(string str)
        {
            if (str == null) return null;
            if (!str.StartsWith("\"")) return str;
            if (!str.EndsWith("\"")) return str;
            return str.Substring(1, str.Length - 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this IHttpResponse source, HttpResponseMessage target, HttpRequestMessage req)
        {
            target.StatusCode = (HttpStatusCode)source.StatusCode;
            //
            // if we have a last-modified on the source and request
            // has a "if-modified-since - check dates and return correct content
            // based on cache status
            //
            if (source.LastModified != null)
            {
                if (req.Headers.IfModifiedSince != null)
                {
                    var lastModified = source.LastModified.Value.ToUniversalTime().Ticks / 10000000;
                    var ifModifiedSince = req.Headers.IfModifiedSince.Value.ToUniversalTime().Ticks / 10000000;
                    if (lastModified <= ifModifiedSince)
                    {
                        target.StatusCode = HttpStatusCode.NotModified;
                    }
                }
            }

            if (!string.IsNullOrEmpty(source.MediaType))
            {
                if(target.StatusCode == HttpStatusCode.NotModified)
                {
                    source.ResponseStream.Dispose();
                    target.Content = new StreamContent(new MemoryStream());
                }
                else
                {
                    target.Content = new StreamContent(source.ResponseStream);
                }
                target.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(source.MediaType);
            }
            if (!string.IsNullOrEmpty(source.Filename))
            {
                target.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = source.Filename };
            }
            if(source.LastModified != null)
            {
                target.Content.Headers.LastModified = source.LastModified;

                // we have a last modified date - set the cache control to one hour
                target.Headers.CacheControl = new CacheControlHeaderValue();
                target.Headers.CacheControl.Private = true;
                target.Headers.CacheControl.MaxAge = new TimeSpan(24, 0, 0);
            }

            if (source.Location != null)
            {
                target.StatusCode = HttpStatusCode.Redirect;
                target.Headers.Location = new Uri(source.Location);
            }

            if (source.ETag != null)
            {
                target.Headers.ETag = EntityTagHeaderValue.Parse(AddQuotesIfMissing(source.ETag));
            }

            foreach (var header in source.AdditionalHeaders)
            {
                target.Headers.Add(header.Key, header.Value);
            }

            return Task.CompletedTask;
        }

        private static string AddQuotesIfMissing(string eTag)
        {
            if (eTag.StartsWith("\"")) return eTag;
            return $"\"{eTag}\"";
        }
    }
}
