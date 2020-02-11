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
            target.ContentType = source.ContentType;
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
            if (!string.IsNullOrEmpty(source.ContentType))
            {
                target.ContentType = source.ContentType;
                await source.ResponseStream.CopyToAsync(target.Body);
            }
            if(!string.IsNullOrEmpty(source.Location))
            {
                target.StatusCode = 302;
                target.Headers.Add("Location", source.Location);
            }
        }
    }
}
