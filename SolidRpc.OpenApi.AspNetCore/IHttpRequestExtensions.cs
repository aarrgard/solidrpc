using Microsoft.AspNetCore.Http.Features;
using SolidRpc.Abstractions.OpenApi.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="prefixMappings"></param>
        /// <returns></returns>
        public static async Task CopyFromAsync(this IHttpRequest target, Microsoft.AspNetCore.Http.HttpRequest source, IDictionary<string, string> prefixMappings = null)
        {
            target.Scheme = source.Scheme;
            target.Method = source.Method;
            target.HostAndPort = source.Host.ToString();

            //
            // The kestrel team dont like encoded values in the path som we cannot use it to bind stuff...
            // https://github.com/aspnet/Mvc/issues/6388 - its a wont - fix 2017...
            //
            var rawTarget = ((IHttpRequestFeature)source.HttpContext.Features[typeof(IHttpRequestFeature)]).RawTarget;
            target.Path = rawTarget;
            //target.Path = $"{source.PathBase}{source.Path}";


            if (prefixMappings != null)
            {
                foreach (var prefixMapping in prefixMappings)
                {
                    if (target.Path.StartsWith(prefixMapping.Key))
                    {
                        target.Path = $"{prefixMapping.Value}{target.Path.Substring(prefixMapping.Key.Length)}";
                    }
                }
            }

            // extract headers
            var headerList = new List<IHttpRequestData>();
            foreach (var h in source.Headers)
            {
                foreach (var sv in h.Value)
                {
                    headerList.Add(new SolidHttpRequestDataString("text/plain", h.Key, sv));
                }
            }
            target.Headers = headerList;

            // extract query
            var queryList = new List<IHttpRequestData>();
            foreach(var q in source.Query)
            {
                foreach(var sv in q.Value)
                {
                    queryList.Add(new SolidHttpRequestDataString("text/plain", q.Key, sv));
                }
            }
            target.Query = queryList;

            if(source.ContentType != null)
            {
                var mediaType = MediaTypeHeaderValue.Parse(source.ContentType);
                target.ContentType = mediaType.MediaType;
                target.BodyData = await SolidHttpRequestData.ExtractContentData(mediaType, source.Body);
            }
        }

    }
}
