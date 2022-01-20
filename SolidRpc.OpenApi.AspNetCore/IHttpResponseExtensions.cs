using Microsoft.Extensions.Primitives;
using SolidRpc.Abstractions.OpenApi.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IHttpResponseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static async Task CopyFromAsync(this IHttpResponse target, Microsoft.AspNetCore.Http.HttpResponse source)
        {
            target.StatusCode = source.StatusCode;
            target.MediaType = source.ContentType;
            if (source.ContentLength != null)
            {
                var ms = new MemoryStream();
                await source.Body.CopyToAsync(ms);
                target.ResponseStream = new MemoryStream(ms.ToArray());
            }
            if (source.Headers.TryGetValue("Content-Disposition", out StringValues cds))
            {
                var cd = ContentDispositionHeaderValue.Parse(cds);
                target.Filename = cd.FileName;
            }
            if (source.Headers.TryGetValue("Location", out StringValues loc))
            {
                target.Location = loc;
            }
            if (source.Headers.TryGetValue("ETag", out StringValues etag))
            {
                target.ETag = etag;
            }
            if (source.Headers.TryGetValue("Set-Cookie", out StringValues setCookie))
            {
                target.SetCookie = setCookie;
            }
            if (source.Headers.TryGetValue("LastModified", out StringValues lastModified))
            {
                throw new System.Exception("!!!");
                //target.LastModified = etag;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static async Task CopyToAsync(this IHttpResponse source, Microsoft.AspNetCore.Http.HttpResponse target)
        {
            target.StatusCode = source.StatusCode;
            if (!string.IsNullOrEmpty(source.Filename))
            {
                var cd = new ContentDispositionHeaderValue("inline") { FileName = source.Filename };
                target.Headers.Add("Content-Disposition", cd.ToString());
            }
            if (!string.IsNullOrEmpty(source.ETag))
            {
                target.Headers.Add("ETag", AddQuotesIfMissing(source.ETag));
            }
            if (source.LastModified != null)
            {
                target.Headers.Add("Last-Modified", source.LastModified.Value.UtcDateTime.ToString("r"));
            }
            if (!string.IsNullOrEmpty(source.Location))
            {
                target.StatusCode = 302;
                target.Headers.Add("Location", source.Location);
            }
            if (!string.IsNullOrEmpty(source.SetCookie))
            {
                target.Headers.Add("Set-Cookie", source.SetCookie);
            }
            foreach (var header in source.AdditionalHeaders)
            {
                target.Headers[header.Key] = header.Value;
            }
            if (!string.IsNullOrEmpty(source.MediaType))
            {
                var ct = new MediaTypeHeaderValue(source.MediaType);
                if(!string.IsNullOrEmpty(source.CharSet))
                {
                    ct.CharSet = source.CharSet;
                }
                target.ContentType = ct.ToString();
                await source.ResponseStream.CopyToAsync(target.Body);
            }
        }

        private static string AddQuotesIfMissing(string eTag)
        {
            if (eTag.StartsWith("\"")) return eTag;
            return $"\"{eTag}\"";
        }
    }
}
