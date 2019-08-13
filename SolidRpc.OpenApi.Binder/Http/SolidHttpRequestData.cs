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
using System.Threading.Tasks;

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
        private const string SystemUri = "System.Uri";
        private const string SystemString = "System.String";
        private const string SystemIOStream = "System.IO.Stream";
        private const string SystemThreadingCancellationToken = "System.Threading.CancellationToken";
        
        public static readonly IEnumerable<SolidHttpRequestData> EmptyArray = new SolidHttpRequestData[0];

        public static Func<IEnumerable<SolidHttpRequestData>, object, IEnumerable<SolidHttpRequestData>> CreateBinder(string contentType, string name, Type parameterType, string collectionFormat)
        {
            Func<IEnumerable<SolidHttpRequestData>, object, SolidHttpRequestData> subBinder;
            switch(collectionFormat)
            {
                case null:
                    subBinder = CreateBinder(contentType, name, parameterType);
                    return (_, __) => new SolidHttpRequestData[] { subBinder(_, __) };
                case "multi":
                    var binder = CreateEnumBinder(contentType, name, parameterType);
                    return (_, __) => binder(_, __);
                case "csv":
                    var csvBinder = CreateBinder(contentType, name, typeof(string));
                    return (_, __) => csvBinder(_, __).GetStringValue().Split(',').Select(o => new SolidHttpRequestDataString("text/plain", name, o));
                case "ssv":
                    var ssvBinder = CreateBinder(contentType, name, typeof(string));
                    return (_, __) => ssvBinder(_, __).GetStringValue().Split(' ').Select(o => new SolidHttpRequestDataString("text/plain", name, o));
                default:
                    throw new NotImplementedException("cannot handle collection format:" + collectionFormat);
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
                    var data = new SolidHttpRequestDataBinary(sectionMediaType.MediaType, "body", (byte[])null);

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
                            bodyData.Add(new SolidHttpRequestDataString("text/plain", values[0], values[1]));
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
                bodyData.Add(new SolidHttpRequestDataBinary(mediaType.MediaType, "body", ms.ToArray()));
            }
            return bodyData;
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

        private static Func<IEnumerable<SolidHttpRequestData>, object, IEnumerable<SolidHttpRequestData>> CreateEnumBinder(string contentType, string name, Type type)
        {
            var enumType = GetEnumType(type);
            var m = typeof(SolidHttpRequestData).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(o => o.Name == nameof(CreateEnumBinder))
                    .Where(o => o.GetParameters().Length == 2)
                    .Where(o => o.IsGenericMethod)
                    .Single();
            return (Func<IEnumerable<SolidHttpRequestData>, object, IEnumerable<SolidHttpRequestData>>)m.MakeGenericMethod(enumType).Invoke(null, new object[] { contentType, name });
        }

        private static Func<IEnumerable<SolidHttpRequestData>, object, IEnumerable<SolidHttpRequestData>> CreateEnumBinder<T>(string contentType, string name)
        {
            var subBinder = CreateBinder(contentType, name, typeof(T));
            return (_,__) => ((IEnumerable<T>)__).Select(o => subBinder(_, o));
        }

        private static Func<IEnumerable<SolidHttpRequestData>, object, SolidHttpRequestData> CreateBinder(string contentType, string name, Type type)
        {
            if(type?.FullName == SystemIOStream)
            {
                contentType = contentType ?? "application/octet-stream";
                return (_, val) =>
                {
                    var retVal = new SolidHttpRequestDataBinary(contentType, name, (Stream)val);
                    retVal.SetFilename("upload.tmp");
                    return retVal; ;
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
                            return (_, val) => null;
                        case SystemBoolean:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((bool)val) ? "true" : "false");
                        case SystemDouble:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((double)val).ToString(CultureInfo.InvariantCulture));
                        case SystemSingle:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((float)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt16:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((short)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt32:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((int)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt64:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((long)val).ToString(CultureInfo.InvariantCulture));
                        case SystemGuid:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((Guid)val).ToString());
                        case SystemDateTime:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((DateTime)val).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        case SystemUri:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, ((Uri)val).ToString());
                        case SystemString:
                            return (_, val) => new SolidHttpRequestDataString(contentType, name, (string)val);
                        default:
                            throw new NotImplementedException("cannot handle type:" + type.FullName + ":" + contentType);
                    }
                case "application/json":
                    return (_, val) => new SolidHttpRequestDataBinary(contentType, name, JsonHelper.Serialize(val, type));
                default:
                    throw new NotImplementedException("cannot handle content type:" + contentType);
            }
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
        /// The name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Filename { get; protected set; }

        protected Encoding GetEncoding(Encoding encoding)
        {
            return encoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// Returns the string value using supplied encoding.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public abstract string GetStringValue(Encoding encoding = null);

        /// <summary>
        /// Returns the binary value using supplied encoding.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public abstract Stream GetBinaryValue(Encoding encoding = null);

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