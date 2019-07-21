﻿using System;
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

        public static Uri GetUri(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if(request.Host.Port != null)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }
            uriBuilder.Path = $"{request.PathBase}{request.Path}";
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

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
            var headerList = new List<SolidHttpRequestData>();
            foreach (var h in source.Headers)
            {
                foreach (var sv in h.Value)
                {
                    headerList.Add(new HttpRequestDataString("text/plain", h.Key, sv));
                }
            }
            target.Headers = headerList;

            // extract query
            var queryList = new List<SolidHttpRequestData>();
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
                target.BodyData = await SolidHttpRequestData.ExtractContentData(mediaType, source.Body);
            }
        }

    }
}
