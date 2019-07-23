using Microsoft.AspNetCore.Mvc;
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
        public static Task<IActionResult> CreateActionResult(this IHttpResponse source)
        {
            if(source.StatusCode == 200 && source.ContentType != null)
            {
                return Task.FromResult<IActionResult>(new FileStreamResult(source.ResponseStream, source.ContentType));
            }
            else
            {
                return Task.FromResult<IActionResult>(new StatusCodeResult(source.StatusCode));
            }
        }
    }
}
