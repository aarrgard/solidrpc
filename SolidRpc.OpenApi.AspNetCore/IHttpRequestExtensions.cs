using SolidRpc.OpenApi.Binder.Multipart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
            target.Path = source.Path;
            if(source.ContentType != null)
            {
                var httpRequestData = new List<HttpRequestData>();
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

                        httpRequestData.Add(data);

                        section = await reader.ReadNextSectionAsync();
                    }
                }
                else if (mediaType.MediaType == "application/x-www-form-urlencoded")
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var ms = new MemoryStream();
                    await source.Body.CopyToAsync(ms);
                    httpRequestData.Add(new HttpRequestDataBinary(mediaType.MediaType, "body", ms.ToArray()));
                }
                target.BodyData = httpRequestData;
            }
        }
    }
}
