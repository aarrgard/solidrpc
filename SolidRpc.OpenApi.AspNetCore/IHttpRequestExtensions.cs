using SolidRpc.OpenApi.Binder.Multipart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder
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

            // extract body
            if(source.ContentType != null)
            {
                var bodyData = new List<HttpRequestData>();
                var mediaType = MediaTypeHeaderValue.Parse(source.ContentType);
                target.ContentType = mediaType.MediaType;
                if (mediaType.MediaType == "multipart/form-data")
                {
                    var boundary = MultipartRequestHelper.GetBoundary(mediaType, 70);
                    var reader = new MultipartReader(boundary, source.Body);
                    var section = await reader.ReadNextSectionAsync();
                    while (section != null)
                    {
                        var sectionMediaType = section.Headers.ContentType;
                        var data = new HttpRequestDataBinary(sectionMediaType.MediaType, "body", null);

                        var stream = await section.ReadAsStreamAsync();
                        data.SetBinaryData(section.Headers.ContentDisposition?.Name, stream);

                        data.SetFilename(section.Headers.ContentDisposition?.FileName);

                        bodyData.Add(data);

                        section = await reader.ReadNextSectionAsync();
                    }
                }
                else if (mediaType.MediaType == "application/x-www-form-urlencoded")
                {
                    // read the content as a query string
                    StreamReader sr;
                    if(mediaType.CharSet != null)
                    {
                        sr = new StreamReader(source.Body, Encoding.GetEncoding(mediaType.CharSet));
                    }
                    else
                    {
                        sr = new StreamReader(source.Body);
                    }
                    using (sr)
                    {
                        var content = await sr.ReadToEndAsync();
                        content.Split('&').ToList().ForEach(o =>
                        {
                            var values = o.Split('=');
                            if(values.Length != 2)
                            {
                                throw new Exception("Cannot split values");
                            }
                            bodyData.Add(new HttpRequestDataString("text/plain", values[0], values[1]));

                        });
                    }
                }
                else
                {
                    var ms = new MemoryStream();
                    await source.Body.CopyToAsync(ms);
                    bodyData.Add(new HttpRequestDataBinary(mediaType.MediaType, "body", ms.ToArray()));
                }
                target.BodyData = bodyData;
            }
        }
    }
}
