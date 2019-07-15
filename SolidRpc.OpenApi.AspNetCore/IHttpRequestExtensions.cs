using SolidRpc.OpenApi.Binder.Http.Multipart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IHttpRequestExtensions
    {
         /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public static async Task CopyFromAsync(this IHttpRequest target, Microsoft.AspNetCore.Http.HttpRequest source)
        {
            target.Scheme = source.Scheme;
            target.Method = source.Method;
            target.HostAndPort = source.Host.ToString();
            target.Path = $"{source.PathBase}{source.Path}";

            // extract headers
            var headerList = new List<HttpRequestData>();
            foreach (var h in source.Headers)
            {
                foreach (var sv in h.Value)
                {
                    headerList.Add(new HttpRequestDataString("text/plain", h.Key, sv));
                }
            }
            target.Headers = headerList;

            // extract query
            var queryList = new List<HttpRequestData>();
            foreach(var q in source.Query)
            {
                foreach(var sv in q.Value)
                {
                    queryList.Add(new HttpRequestDataString("text/plain", q.Key, sv));
                }
            }
            target.Query = queryList;

            if(source.ContentType != null)
            {
                var mediaType = MediaTypeHeaderValue.Parse(source.ContentType);
                target.ContentType = mediaType.MediaType;
                target.BodyData = await HttpRequestData.ExtractContentData(mediaType, source.Body);
            }
        }

    }
}
