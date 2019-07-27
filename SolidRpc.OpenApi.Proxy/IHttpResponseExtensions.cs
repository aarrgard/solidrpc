using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IHttpResponseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static async Task CopyFromAsync(this IHttpResponse target, HttpResponseMessage source)
        {
            target.StatusCode = (int)source.StatusCode;
            if (source.Content != null)
            {
                target.ContentType = source.Content.Headers?.ContentType?.MediaType;
                var ms = new MemoryStream();
                await source.Content.CopyToAsync(ms);
                target.ResponseStream = new MemoryStream(ms.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this IHttpResponse source, HttpResponseMessage target)
        {
            target.StatusCode = (HttpStatusCode)source.StatusCode;
            if (!string.IsNullOrEmpty(source.ContentType))
            {
                target.Content = new StreamContent(source.ResponseStream);
                target.Content.Headers.ContentType = new MediaTypeHeaderValue(source.ContentType);
            }
            return Task.CompletedTask;
        }

    }
}
