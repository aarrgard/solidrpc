using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IHttpRequestExtensions
    {
        /// <summary>
        /// Returns the host
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetHost(this IHttpRequest source)
        {
            var colonIdx = source.HostAndPort.IndexOf(':');
            if (colonIdx == -1)
            {
                return source.HostAndPort;
            }
            return source.HostAndPort.Substring(0, colonIdx);
        }

        /// <summary>
        /// Returns the port
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? GetPort(this IHttpRequest source)
        {
            var colonIdx = source.HostAndPort.IndexOf(':');
            if (colonIdx == -1)
            {
                return null;
            }
            return int.Parse(source.HostAndPort.Substring(colonIdx+1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyTo(this IHttpRequest source, HttpRequestMessage target)
        {
            var builder = new UriBuilder
            {
                Scheme = source.Scheme,
                Host = source.GetHost(),
                Path = source.Path,
                Query = string.Join("&", source.Query.Select(o => $"{HttpUtility.UrlEncode(o.Name)}={HttpUtility.UrlEncode(o.GetStringValue())}"))
            };
            var port = source.GetPort();
            if (port != null)
            {
                builder.Port = port.Value;
            }

            target.Method = new HttpMethod(source.Method);
            target.RequestUri = builder.Uri;

            source.Headers.ToList().ForEach(o => target.Headers.Add(o.Name, o.GetStringValue()));

            switch (source.ContentType?.ToLower())
            {
                case null:
                    break;
                case "application/x-www-form-urlencoded":
                    target.Content = CreateFormUrlEncodedContent(source.BodyData);
                    break;
                case "multipart/form-data":
                    target.Content = CreateMultipartFormDataContent(source.BodyData);
                    break;
                default:
                    target.Content = CreateBody(source.BodyData);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static async Task CopyFromAsync(this IHttpRequest target, HttpRequestMessage source)
        {
            target.Method = source.Method.Method;
            var uri = source.RequestUri;
            if(uri.IsDefaultPort)
            {
                target.HostAndPort = uri.Host;
            }
            else
            {
                target.HostAndPort = $"{uri.Host}:{uri.Port}";
            }
            target.HostAndPort = uri.Host;
            target.Path = uri.AbsolutePath;

            target.Query = (uri.Query.StartsWith("?") ? uri.Query.Substring(1) : uri.Query)
                .Split('&')
                .Select(o => o.Split('='))
                .Where(o => o.Length > 0)
                .Where(o => !string.IsNullOrEmpty(o[0]))
                .Select(o =>
                {
                    if (o.Length == 1)
                    {
                        return new HttpRequestDataString("text/plain", HttpUtility.UrlDecode(o[0]), "");
                    }
                    else if (o.Length == 2)
                    {
                        return new HttpRequestDataString("text/plain", HttpUtility.UrlDecode(o[0]), HttpUtility.UrlDecode(o[1]));
                    }
                    else
                    {
                        throw new Exception("Cannot parse query string");
                    }
                }).ToList();
            target.Headers = source.Headers
                .SelectMany(o => o.Value.Select(o2 => new { o.Key, Value = o2 }))
                .Select(o => new HttpRequestDataString("text/plain", o.Key, o.Value))
                .ToList();

            var mediaType = source.Content?.Headers?.ContentType;
            if(mediaType != null)
            {
                target.ContentType = mediaType.MediaType;
                target.BodyData = await SolidHttpRequestData.ExtractContentData(mediaType, await source.Content.ReadAsStreamAsync());
            }
        }

        private static HttpContent CreateMultipartFormDataContent(IEnumerable<IHttpRequestData> bodyData)
        {
            var content = new MultipartFormDataContent();
            foreach(var d in bodyData)
            {
                HttpContent part;
                if (d is SolidHttpRequestDataBinary binary)
                {
                    part = new StreamContent(d.GetBinaryValue());
                    part.Headers.ContentType = new MediaTypeHeaderValue(binary.ContentType);
                    if(binary.Filename == null)
                    {
                        content.Add(part, binary.Name);
                    }
                    else
                    {
                        content.Add(part, binary.Name, binary.Filename);
                    }
                }
                else if (d is HttpRequestDataString)
                {
                    part = new StringContent(d.GetStringValue());
                    content.Add(part, d.Name);
                }
                else
                {
                    throw new Exception("Cannot handle data");
                }
            }
            return content;
        }

        private static HttpContent CreateFormUrlEncodedContent(IEnumerable<IHttpRequestData> bodyData)
        {
            FormUrlEncodedContent formContent;
            var data = bodyData.Select(o => new KeyValuePair<string, string>(o.Name, o.GetStringValue()));
            formContent = new FormUrlEncodedContent(data);
            return formContent;
        }

        private static HttpContent CreateBody(IEnumerable<IHttpRequestData> bodyData)
        {
            if(bodyData.Count() != 1)
            {
                throw new Exception("Cannot create body from more than one data.");
            }
            var body = bodyData.First();
            HttpContent content;
            switch (body.ContentType.ToLower())
            {
                case "application/octet-stream":
                    content = new StreamContent(body.GetBinaryValue());
                    content.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = body.Name, FileName = "test.xml" };
                    break;
                case "application/json":
                    var enc = Encoding.UTF8;
                    content = new StringContent(body.GetStringValue(enc));
                    content.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType) { CharSet = enc.HeaderName };
                    break;
                default:
                    throw new Exception("Cannot handle content type:" + body.ContentType);
            }
            return content;
        }


    }
}
