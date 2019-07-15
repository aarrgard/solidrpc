using System.IO;
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
        /// <param name="request"></param>
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public static async Task CopyToAsync(this IHttpResponse source, Microsoft.AspNetCore.Http.HttpResponse target)
        {
            target.StatusCode = source.StatusCode;
            if(!string.IsNullOrEmpty(source.ContentType))
            {
                target.ContentType = source.ContentType;
                await source.ResponseStream.CopyToAsync(target.Body);
            }
        }
    }
}
