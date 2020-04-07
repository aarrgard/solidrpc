using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Collections.Generic;
using System.IO;
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
            return int.Parse(source.HostAndPort.Substring(colonIdx + 1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static async Task CopyToAsync(this IHttpRequest source, HttpRequest target)
        {
            target.Method = source.Method;
            target.Uri = CreateUri(source);

            target.Headers = source.Headers.GroupBy(o => o.Name)
                .ToDictionary(o => o.Key, o => o.Select(o2 => o2.GetStringValue()).ToArray());

            var content = CreateContent(source.ContentType, source.BodyData);
            if (content != null)
            {
                content.Headers.ToList().ForEach(o => { target.Headers[o.Key] = o.Value.ToArray(); });
                target.Body = await content.ReadAsStreamAsync();
            }
        }

        /// <summary>
        /// Consturcts a uri based on the information in the http request data.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Uri CreateUri(this IHttpRequest source, bool includeQueryString = true)
        {
            var builder = new UriBuilder
            {
                Scheme = source.Scheme,
                Host = source.GetHost(),
                Path = source.Path
            };
            if(includeQueryString)
            {
                builder.Query = string.Join("&", source.Query.Select(o => $"{HttpUtility.UrlEncode(o.Name)}={HttpUtility.UrlEncode(o.GetStringValue())}"));
            }
            var port = source.GetPort();
            if (port != null)
            {
                builder.Port = port.Value;
            }
            return builder.Uri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyTo(this IHttpRequest source, HttpRequestMessage target)
        {
            target.Method = new HttpMethod(source.Method);
            target.RequestUri = source.CreateUri();
            source.Headers.ToList().ForEach(o => target.Headers.Add(o.Name, o.GetStringValue()));

            target.Content = CreateContent(source.ContentType, source.BodyData);
        }

        private static HttpContent CreateContent(string contentType, IEnumerable<IHttpRequestData> bodyData)
        {
            switch (contentType?.ToLower())
            {
                case null:
                    return null;
                case "application/x-www-form-urlencoded":
                    return CreateFormUrlEncodedContent(bodyData);
                case "multipart/form-data":
                    return CreateMultipartFormDataContent(bodyData);
                default:
                    return CreateBody(bodyData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="prefixMappings"></param>
        public static async Task CopyFromAsync(this IHttpRequest target, HttpRequest source)
        {
            target.Method = source.Method;
            target.CopyUri(source.Uri);
            target.Headers = source.Headers
                .SelectMany(o => o.Value.Select(o2 => new { o.Key, Value = o2 }))
                .Select(o => new SolidHttpRequestDataString("text/plain", o.Key, o.Value))
                .ToList();

            source.Headers.TryGetValue("Content-Type", out string[] contentTypeValues);
            var contentTypeValue = contentTypeValues?.FirstOrDefault();
            if (contentTypeValue != null)
            {
                var mediaType = MediaTypeHeaderValue.Parse(contentTypeValue);
                target.ContentType = mediaType.MediaType;
                target.BodyData = await SolidHttpRequestData.ExtractContentData(mediaType, source.Body);
            }
        }

        /// <summary>
        /// Copies the uri data to the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uri"></param>
        /// <param name="prefixMappings"></param>
        public static void CopyUri(this IHttpRequest target, Uri uri, IDictionary<string, string> prefixMappings = null)
        {
            if (uri.IsDefaultPort)
            {
                target.HostAndPort = uri.Host;
            }
            else
            {
                target.HostAndPort = $"{uri.Host}:{uri.Port}";
            }
            target.HostAndPort = uri.Host;
            target.Path = uri.AbsolutePath;
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
            target.Query = (uri.Query.StartsWith("?") ? uri.Query.Substring(1) : uri.Query)
                .Split('&')
                .Select(o => o.Split('='))
                .Where(o => o.Length > 0)
                .Where(o => !string.IsNullOrEmpty(o[0]))
                .Select(o =>
                {
                    if (o.Length == 1)
                    {
                        return new SolidHttpRequestDataString("text/plain", HttpUtility.UrlDecode(o[0]), "");
                    }
                    else if (o.Length == 2)
                    {
                        return new SolidHttpRequestDataString("text/plain", HttpUtility.UrlDecode(o[0]), HttpUtility.UrlDecode(o[1]));
                    }
                    else
                    {
                        throw new Exception("Cannot parse query string");
                    }
                }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="prefixMappings"></param>
        public static async Task CopyFromAsync(this IHttpRequest target, HttpRequestMessage source, IDictionary<string, string> prefixMappings = null)
        {
            target.Method = source.Method.Method;
            target.CopyUri(source.RequestUri, prefixMappings);
            target.Headers = source.Headers
                .SelectMany(o => o.Value.Select(o2 => new { o.Key, Value = o2 }))
                .Select(o => new SolidHttpRequestDataString("text/plain", o.Key, o.Value))
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
                    var streamContent = d.GetBinaryValue();
                    if (streamContent == null) continue;
                    part = new StreamContent(streamContent);
                    var contentType = binary.ContentType ?? "application/octet-stream";
                    part.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    if(binary.Encoding != null)
                    {
                        part.Headers.ContentType.CharSet = binary.Encoding.HeaderName;
                    }
                    if(!string.IsNullOrEmpty(d.ETag))
                    {
                        part.Headers.Add("X-ETag", d.ETag);
                    }
                    if (string.IsNullOrEmpty(binary.Filename))
                    {
                        content.Add(part, binary.Name);
                    }
                    else
                    {
                        content.Add(part, binary.Name, binary.Filename);
                    }
                }
                else if (d is SolidHttpRequestDataString)
                {
                    part = new StringContent(d.GetStringValue());
                    content.Add(part, d.Name);
                }
                else
                {
                    throw new Exception("Cannot handle data");
                }
            }
            // return null if no contents were added.
            if (!content.Any()) return null;
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
            var contentTypes = bodyData.Select(o => o.ContentType).Distinct();
            if(contentTypes.Count() != 1)
            {
                throw new Exception($"Cannot create body from more than one content-types({string.Join(",", contentTypes)})");
            }
            var body = bodyData.First();
            HttpContent content;
            switch (body.ContentType.ToLower())
            {
                case "application/octet-stream":
                    content = new StreamContent(body.GetBinaryValue());
                    content.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType) { CharSet = body.Encoding?.HeaderName };
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = body.Name, FileName = "test.xml" };
                    break;
                case "application/json":
                    content = new StringContent(CreateJsonData(bodyData.ToList()));
                    content.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType) { CharSet = body.Encoding?.HeaderName };
                    break;
                default:
                    content = new StreamContent(body.GetBinaryValue());
                    content.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType) { CharSet = body.Encoding?.HeaderName };
                    break;
            }
            return content;
        }

        private static string CreateJsonData(IList<IHttpRequestData> bodyData)
        {
            if(bodyData.Count == 1)
            {
                return bodyData.First().GetStringValue();
            }
            var sb = new StringBuilder("{");
            for(int i = 0; i < bodyData.Count; i++)
            {
                if(i != 0)
                {
                    sb.Append(",");
                }
                sb.Append("\"").Append(bodyData[i].Name).Append("\":").Append(bodyData[i].GetStringValue());
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
