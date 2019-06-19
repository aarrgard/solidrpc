using SolidRpc.Proxy;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.Binder
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
        public static async Task CopyFrom(this IHttpResponse target, HttpResponseMessage source)
        {
            target.StatusCode = (int)source.StatusCode;
            if(source.Content != null)
            {
                target.ContentType = source.Content.Headers?.ContentType?.MediaType;
                var ms = new MemoryStream();
                await source.Content.CopyToAsync(ms);
                target.ResponseStream = new MemoryStream(ms.ToArray());
            }
        }

    }
}
