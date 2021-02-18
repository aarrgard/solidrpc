using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http.Multipart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public abstract class SolidHttpRequestData : IHttpRequestData
    {
        private const string SystemBoolean = "System.Boolean";
        private const string SystemDouble = "System.Double";
        private const string SystemByte = "System.Byte";
        private const string SystemByteArray = "System.Byte[]";
        private const string SystemSingle = "System.Single";
        private const string SystemInt16 = "System.Int16";
        private const string SystemInt32 = "System.Int32";
        private const string SystemInt64 = "System.Int64";
        private const string SystemGuid = "System.Guid";
        private const string SystemDateTime = "System.DateTime";
        private const string SystemDateTimeOffset = "System.DateTimeOffset";
        private const string SystemTimeSpan = "System.TimeSpan";
        private const string SystemUri = "System.Uri";
        private const string SystemString = "System.String";
        private const string SystemIOStream = "System.IO.Stream";
        private const string SystemThreadingCancellationToken = "System.Threading.CancellationToken";
        private const string SystemSecurityPrincipalIPrincipal = "System.Security.Principal.IPrincipal";
        private static readonly ICollection<string> SystemTypes = new HashSet<string>()
        {
            SystemBoolean, SystemDouble, SystemByte, SystemSingle,
            SystemInt16, SystemInt32, SystemInt64, SystemGuid,
            SystemDateTime, SystemDateTimeOffset, SystemTimeSpan,
            SystemUri, SystemString, SystemIOStream, SystemThreadingCancellationToken
        };
        public static readonly IEnumerable<IHttpRequestData> EmptyArray = new SolidHttpRequestData[0];

        public static bool IsSimpleType(Type type)
        {
            if (type.IsEnumType(out Type enumType))
            {
                return IsSimpleType(enumType);
            }
            if (type.IsTaskType(out Type taskType))
            {
                return IsSimpleType(taskType);
            }
            return SystemTypes.Contains(type.FullName);
        }

        public static Func<IEnumerable<IHttpRequestData>, object, Task<IEnumerable<IHttpRequestData>>> CreateBinder(string contentType, string name, Type parameterType, string format, string collectionFormat)
        {
            Func<IEnumerable<IHttpRequestData>, object, Task<IHttpRequestData>> subBinder;
            switch (collectionFormat)
            {
                case null:
                    subBinder = CreateBinder(contentType, name, parameterType, format);
                    return async (_, __) => new IHttpRequestData[] { await subBinder(_, __) };
                case "multi":
                    var binder = CreateEnumBinder(contentType, name, parameterType, format);
                    return (_, __) => binder(_, __);
                case "csv":
                    var csvBinder = CreateEnumBinder(contentType, name, parameterType, format);
                    return async (_, __) => new IHttpRequestData[] { new SolidHttpRequestDataString("text/plain", name, string.Join(",", (await csvBinder(_, __)).Select(o => o.GetStringValue()))) };
                case "ssv":
                    var ssvBinder = CreateEnumBinder(contentType, name, parameterType, format);
                    return async (_, __) => new IHttpRequestData[] { new SolidHttpRequestDataString("text/plain", name, string.Join(" ", (await ssvBinder(_, __)).Select(o => o.GetStringValue()))) };
                case "pipes":
                    var pipesBinder = CreateEnumBinder(contentType, name, parameterType, format);
                    return async (_, __) => new IHttpRequestData[] { new SolidHttpRequestDataString("text/plain", name, string.Join("|", (await pipesBinder(_, __)).Select(o => o.GetStringValue()))) };
                case "tsv":
                    var tsvBinder = CreateEnumBinder(contentType, name, parameterType, format);
                    return async (_, __) => new IHttpRequestData[] { new SolidHttpRequestDataString("text/plain", name, string.Join("\t", (await tsvBinder(_, __)).Select(o => o.GetStringValue()))) };
                default:
                    throw new NotImplementedException("cannot handle collection format:" + collectionFormat);
            }
        }

        public static Func<IEnumerable<IHttpRequestData>, object> CreateExtractor(string contentType, string name, Type parameterType, string format, string collectionFormat)
        {
            Func<IHttpRequestData, object> subExtractor;
            switch (collectionFormat)
            {
                case null:
                    subExtractor = CreateExtractor(contentType, name, parameterType, !parameterType.IsValueType);
                    return (_) => subExtractor(_.FirstOrDefault());
                case "multi":
                    var enumExtractor = CreateEnumExtractor(contentType, name, parameterType);
                    return (_) => { return enumExtractor(_); };
                case "csv":
                    var csvExtractor = CreateEnumExtractor(contentType, name, parameterType);
                    return (_) => { return csvExtractor(_.SelectMany(o => o.GetStringValue().Split(',').Select(o2 => new SolidHttpRequestDataString(o.ContentType, o.Name, o2)))); };
                case "ssv":
                    var ssvExtractor = CreateEnumExtractor(contentType, name, parameterType);
                    return (_) => { return ssvExtractor(_.SelectMany(o => o.GetStringValue().Split(' ').Select(o2 => new SolidHttpRequestDataString(o.ContentType, o.Name, o2)))); };
                case "pipes":
                    var pipesExtractor = CreateEnumExtractor(contentType, name, parameterType);
                    return (_) => { return pipesExtractor(_.SelectMany(o => o.GetStringValue().Split('|').Select(o2 => new SolidHttpRequestDataString(o.ContentType, o.Name, o2)))); };
                case "tsv":
                    var tsvExtractor = CreateEnumExtractor(contentType, name, parameterType);
                    return (_) => { return tsvExtractor(_.SelectMany(o => o.GetStringValue().Split('\t').Select(o2 => new SolidHttpRequestDataString(o.ContentType, o.Name, o2)))); };
                default:
                    throw new NotImplementedException("cannot handle collection format:" + collectionFormat);
            }
        }

        private static Func<IEnumerable<IHttpRequestData>, object> CreateEnumExtractor(string contentType, string name, Type type)
        {
            var enumType = GetEnumType(type);
            var m = typeof(SolidHttpRequestData).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                     .Where(o => o.Name == nameof(CreateEnumExtractor))
                     .Where(o => o.GetParameters().Length == 2)
                     .Where(o => o.IsGenericMethod)
                     .Single();
            var arrFunc = (Func<IEnumerable<IHttpRequestData>, object>)m.MakeGenericMethod(enumType).Invoke(null, new object[] { contentType, name });

            if(!type.IsAssignableFrom(enumType.MakeArrayType()))
            {
                if(type == typeof(StringValues))
                {
                    var oldArrFunc = arrFunc;
                    arrFunc = _ => new StringValues((string[])oldArrFunc(_));
                }
                else
                {
                    throw new Exception($"Cannot assign {enumType}[] to {type}");
                }
            }

            return arrFunc;
        }

        private static Func<IEnumerable<IHttpRequestData>, IEnumerable<T>> CreateEnumExtractor<T>(string contentType, string name)
        {
            var subExtractor = CreateExtractor(contentType, name, typeof(T), !typeof(T).IsValueType);
            return (_) => {
                var arr = _.Select(o => (T)subExtractor(o)).ToArray();
                return arr;
            };
        }


        private static Func<IHttpRequestData, object> CreateExtractor(string contentType, string name, Type type, bool nullable)
        {
            if (type.IsNullableType(out Type nullableType))
            {
                var subExtractor = CreateExtractor(contentType, name, nullableType, true);
                return (_) =>
                {
                     return subExtractor(_);
                };
            }
            contentType = contentType ?? "application/json";
            switch (contentType)
            {
                case "text/plain":
                    switch (type?.FullName)
                    {
                        case null:
                        case SystemThreadingCancellationToken:
                            return (_) => nullable ? (CancellationToken?)null : CancellationToken.None;
                        case SystemSecurityPrincipalIPrincipal:
                            return (_) => null;
                        case SystemBoolean:
                            return (_) => {
                                if (bool.TryParse(_?.GetStringValue(), out bool parsed)) 
                                {
                                    return parsed;
                                } 
                                else 
                                {
                                    return nullable ? (bool?)null : false;
                                };
                            };
                        case SystemDouble:
                            return (_) => {
                                if (double.TryParse(_?.GetStringValue(), out double parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (double?)null : 0;
                                };
                            };
                        case SystemSingle:
                            return (_) => {
                                if (float.TryParse(_?.GetStringValue(), out float parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (float?)null : 0;
                                };
                            };
                        case SystemInt16:
                            return (_) => {
                                if (short.TryParse(_?.GetStringValue(), out short parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (short?)null : 0;
                                };
                            };
                        case SystemInt32:
                            return (_) => {
                                if (int.TryParse(_?.GetStringValue(), out int parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (int?)null : 0;
                                };
                            };
                        case SystemInt64:
                            return (_) => {
                                if (long.TryParse(_?.GetStringValue(), out long parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (long?)null : 0;
                                };
                            };
                        case SystemGuid:
                            return (_) => {
                                if (Guid.TryParse(_?.GetStringValue(), out Guid parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (Guid?)null : Guid.Empty;
                                };
                            };
                        case SystemDateTime:
                            return (_) => {
                                if (DateTime.TryParse(_?.GetStringValue(), out DateTime parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (DateTime?)null : DateTime.MinValue;
                                };
                            };
                        case SystemDateTimeOffset:
                            return (_) => {
                                if (DateTimeOffset.TryParseExact(_?.GetStringValue(), "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset parsed1))
                                {
                                    return parsed1;
                                }
                                else if (DateTime.TryParse(_?.GetStringValue(), out DateTime parsed2))
                                {
                                    return ((DateTimeOffset)parsed2);
                                }
                                else
                                {
                                    return nullable ? (DateTimeOffset?)null : DateTimeOffset.MinValue;
                                };
                            };
                        case SystemTimeSpan:
                            return (_) => {
                                if (TimeSpan.TryParse(_?.GetStringValue(), CultureInfo.InvariantCulture, out TimeSpan parsed))
                                {
                                    return parsed;
                                }
                                else
                                {
                                    return nullable ? (TimeSpan?)null : TimeSpan.MinValue;
                                };
                            };
                        case SystemUri:
                            return (_) => _ == null ? null : new Uri(_.GetStringValue());
                        case SystemString:
                            return (_) => _?.GetStringValue();
                        default:
                            if (type?.IsEnum ?? false)
                            {
                                return (_) => Enum.Parse(type, _.GetStringValue());
                            }
                            throw new NotImplementedException("cannot handle type:" + type.FullName + ":" + contentType);
                    }
                case "application/json":
                    return (_) =>
                    {
                        if (_ == null) return null;
                        using (var s = _.GetBinaryValue())
                        {
                            return JsonHelper.Deserialize(s, type);
                        }
                    };
                default:
                    throw new NotImplementedException("cannot handle content type:" + contentType);
            }
        }

        public static async Task<IEnumerable<SolidHttpRequestData>> ExtractContentData(MediaTypeHeaderValue mediaType, Stream body)
        {
            // extract body
            if (mediaType == null)
            {
                return null;
            }
            var bodyData = new List<SolidHttpRequestData>();
            if (mediaType.MediaType == "multipart/form-data")
            {
                var boundary = MultipartRequestHelper.GetBoundary(mediaType, 70);
                var reader = new MultipartReader(boundary, body);
                var section = await reader.ReadNextSectionAsync();
                while (section != null)
                {
                    var sectionMediaType = section.Headers.ContentType;
                    var data = new SolidHttpRequestDataBinary(sectionMediaType.MediaType, sectionMediaType.CharSet, "body", (byte[])null);

                    var stream = await section.ReadAsStreamAsync();
                    await data.SetBinaryData(StripQuotes(section.Headers.ContentDisposition?.Name), stream);

                    data.SetFilename(StripQuotes(section.Headers.ContentDisposition?.FileName));

                    if (section.Headers.TryGetValues("X-ETag", out IEnumerable<string> etag))
                    {
                        data.SetETag(StripQuotes(string.Join("", etag)));
                    }

                    if (section.Headers.TryGetValues("X-LastModified", out IEnumerable<string> lastModified))
                    {
                        if(DateTimeOffset.TryParseExact(string.Join("", lastModified), "r", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dto))
                        {
                            data.SetLastModified(dto.ToLocalTime());
                        }
                    }

                    bodyData.Add(data);

                    section = await reader.ReadNextSectionAsync();
                }
            }
            else if (mediaType.MediaType == "application/x-www-form-urlencoded")
            {
                // read the content as a query string
                StreamReader sr;
                if (mediaType.CharSet != null)
                {
                    sr = new StreamReader(body, Encoding.GetEncoding(mediaType.CharSet));
                }
                else
                {
                    sr = new StreamReader(body);
                }
                using (sr)
                {
                    var content = await sr.ReadToEndAsync();
                    content.Split('&').ToList().ForEach(o =>
                    {
                        var values = o.Split('=');
                        if(values.Length == 0)
                        {
                            // no data
                            return;
                        }
                        if (values.Length == 1)
                        {
                            bodyData.Add(new SolidHttpRequestDataString("text/plain", values[0], true.ToString()));
                        }
                        else if (values.Length == 2)
                        {
                            bodyData.Add(new SolidHttpRequestDataString("text/plain", values[0], HttpUtility.UrlDecode(values[1])));
                        }
                        else
                        {
                            throw new Exception("Cannot split values");
                        }

                    });
                }
            }
            else
            {
                var ms = new MemoryStream();
                await body.CopyToAsync(ms);
                bodyData.Add(new SolidHttpRequestDataBinary(mediaType.MediaType, mediaType.CharSet, "body", ms.ToArray()));
            }
            return bodyData;
        }

        private static string StripQuotes(string str)
        {
            if (str == null) return null;
            if (str.Length < 2) return str;
            if (str[0] != '"') return str;
            if (str[str.Length - 1] != '"') return str;
            return str.Substring(1,str.Length-2);
        }

        private static Type GetEnumType(Type type)
        {
            if (type.IsGenericType)
            {
                if (typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return type.GetGenericArguments()[0];
                }
            }
            return type.GetInterfaces().Select(o => GetEnumType(o)).Where(o => o != null).FirstOrDefault();
        }

        private static Func<IEnumerable<IHttpRequestData>, object, Task<IEnumerable<IHttpRequestData>>> CreateEnumBinder(string contentType, string name, Type type, string format)
        {
            var enumType = GetEnumType(type);
            var m = typeof(SolidHttpRequestData).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(o => o.Name == nameof(CreateEnumBinder))
                    .Where(o => o.GetParameters().Length == 3)
                    .Where(o => o.IsGenericMethod)
                    .Single();
            return (Func<IEnumerable<IHttpRequestData>, object, Task<IEnumerable<IHttpRequestData>>>)m.MakeGenericMethod(enumType).Invoke(null, new object[] { contentType, name, format });
        }

        private static Func<IEnumerable<IHttpRequestData>, object, Task<IEnumerable<IHttpRequestData>>> CreateEnumBinder<T>(string contentType, string name, string format)
        {
            var subBinder = CreateBinder(contentType, name, typeof(T), format);
            return async (rd, e) =>
            {
                if (e == null) return EmptyArray;
                var tasks = ((IEnumerable<T>)e).Select(o => subBinder(rd, o));
                var res = await Task.WhenAll(tasks);
                return res;
            };
        }

        private static Func<IEnumerable<IHttpRequestData>, object, Task<IHttpRequestData>> CreateBinder(string contentType, string name, Type type, string format)
        {
            if(type.IsNullableType(out Type nullableType))
            {
                var subBinder = CreateBinder(contentType, name, nullableType, format);
                return (_, val) =>
                {
                    if(val == null)
                    {
                        return null;

                    }
                    return subBinder(_, val);
                };
            }
            if (type?.FullName == SystemIOStream)
            {
                contentType = contentType ?? "application/octet-stream";
                return (_, val) =>
                {
                    var retVal = new SolidHttpRequestDataBinary(contentType, null, name, (Stream)val);
                    retVal.SetFilename("upload.tmp");
                    return f(retVal);
                };
            }
            contentType = contentType ?? "application/json";
            switch (contentType)
            {
                case "text/plain":
                    switch (type?.FullName)
                    {
                        case null:
                        case SystemThreadingCancellationToken:
                        case SystemSecurityPrincipalIPrincipal:
                            return (_, val) => null;
                        case SystemBoolean:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((bool)val) ? "true" : "false"));
                        case SystemDouble:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((double)val).ToString(CultureInfo.InvariantCulture)));
                        case SystemSingle:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((float)val).ToString(CultureInfo.InvariantCulture)));
                        case SystemInt16:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((short)val).ToString(CultureInfo.InvariantCulture)));
                        case SystemInt32:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((int)val).ToString(CultureInfo.InvariantCulture)));
                        case SystemInt64:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((long)val).ToString(CultureInfo.InvariantCulture)));
                        case SystemGuid:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((Guid)val).ToString()));
                        case SystemDateTime:
                            switch(format?.ToLower())
                            {
                                case "date":
                                    return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((DateTime)val).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                                default:
                                    return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((DateTime)val).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                        case SystemDateTimeOffset:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((DateTimeOffset)val).ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)));
                        case SystemTimeSpan:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((TimeSpan)val).ToString()));
                        case SystemUri:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((Uri)val).ToString()));
                        case SystemString:
                            return (_, val) => f(new SolidHttpRequestDataString(contentType, name, (string)val));
                        default:
                            if(type?.IsEnum ?? false)
                            {
                                return (_, val) => f(new SolidHttpRequestDataString(contentType, name, ((Enum)val).ToString()));
                            }
                            throw new NotImplementedException("cannot handle type:" + type.FullName + ":" + contentType);
                    }
                case "application/json":
                    return (_, val) => f(new SolidHttpRequestDataBinary(contentType, JsonHelper.DefaultEncoding.HeaderName, name, JsonHelper.Serialize(val, type)));
                default:
                    throw new NotImplementedException("cannot handle content type:" + contentType);
            }
        }

        private static Task<IHttpRequestData> f(IHttpRequestData d)
        {
            return Task.FromResult(d);
        }

        /// <summary>
        /// The name of the request data.
        /// </summary>
        /// <param name="name"></param>
        public SolidHttpRequestData(string contentType, string name)
        {
            ContentType = contentType;
            Name = name;
        }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; protected set; }

        /// <summary>
        /// The ETag
        /// </summary>
        public string ETag { get; protected set; }

        /// <summary>
        /// The last modified time of the data
        /// </summary>
        public DateTimeOffset? LastModified { get; protected set; }

        /// <summary>
        /// The content type
        /// </summary>
        public Encoding Encoding { get; protected set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Filename { get; protected set; }

        protected Encoding GetEncoding()
        {
            return Encoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// Returns the string value using supplied encoding.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public abstract string GetStringValue();

        /// <summary>
        /// Returns the binary value using supplied encoding.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public abstract Stream GetBinaryValue();

        /// <summary>
        /// Convertes the value to specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>()
        {
            switch(ContentType?.ToLower())
            {
                case "text/plain":
                case "application/octet-stream":
                    switch (typeof(T).FullName)
                    {
                        case SystemIOStream:
                            return (T)(object)GetBinaryValue();
                        case SystemByteArray:
                            using (var s = GetBinaryValue())
                            {
                                var ms = new MemoryStream();
                                s.CopyTo(ms);
                                return (T)(object)ms.ToArray();
                            }
                        case SystemString:
                            return (T)(object)GetStringValue();
                        default:
                            throw new Exception("Cannot handle:" + typeof(T).FullName);
                    }
                case "application/json":
                    return JsonConvert.DeserializeObject<T>(GetStringValue());
                default:
                    throw new Exception("Cannot handle:"+ContentType);
            }
        }
    }
}