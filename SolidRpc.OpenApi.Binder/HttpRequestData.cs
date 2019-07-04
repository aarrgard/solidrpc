using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public abstract class HttpRequestData
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
        private const string SystemString = "System.String";
        private const string SystemIOStream = "System.IO.Stream";
        private const string SystemThreadingCancellationToken = "System.Threading.CancellationToken";
        
        public static readonly IEnumerable<HttpRequestData> EmptyArray = new HttpRequestData[0];

        public static Func<IEnumerable<HttpRequestData>, object, IEnumerable<HttpRequestData>> CreateBinder(string contentType, string name, Type parameterType, string collectionFormat)
        {
            Func<IEnumerable<HttpRequestData>, object, HttpRequestData> subBinder;
            switch(collectionFormat)
            {
                case null:
                    subBinder = CreateBinder(contentType, name, parameterType);
                    return (_, __) => new HttpRequestData[] { subBinder(_, __) };
                case "multi":
                    var binder = CreateEnumBinder(contentType, name, parameterType);
                    return (_, __) => binder(_, __);
                case "csv":
                    var csvBinder = CreateBinder(contentType, name, typeof(string));
                    return (_, __) => csvBinder(_, __).GetStringValue().Split(';').Select(o => new HttpRequestDataString("text/plain", name, o));
                default:
                    throw new NotImplementedException("cannot handle collection format:" + collectionFormat);
            }
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

        private static Func<IEnumerable<HttpRequestData>, object, IEnumerable<HttpRequestData>> CreateEnumBinder(string contentType, string name, Type type)
        {
            var enumType = GetEnumType(type);
            var m = typeof(HttpRequestData).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(o => o.Name == nameof(CreateEnumBinder))
                    .Where(o => o.GetParameters().Length == 2)
                    .Where(o => o.IsGenericMethod)
                    .Single();
            return (Func<IEnumerable<HttpRequestData>, object, IEnumerable<HttpRequestData>>)m.MakeGenericMethod(enumType).Invoke(null, new object[] { contentType, name });
        }

        private static Func<IEnumerable<HttpRequestData>, object, IEnumerable<HttpRequestData>> CreateEnumBinder<T>(string contentType, string name)
        {
            var subBinder = CreateBinder(contentType, name, typeof(T));
            return (_,__) => ((IEnumerable<T>)__).Select(o => subBinder(_, o));
        }

        private static Func<IEnumerable<HttpRequestData>, object, HttpRequestData> CreateBinder(string contentType, string name, Type type)
        {
            if(type?.FullName == SystemIOStream)
            {
                contentType = contentType ?? "application/octet-stream";
                return (_, val) =>
                {
                    var retVal = new HttpRequestDataBinary(contentType, name, (Stream)val);
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
                            return (_, val) => new HttpRequestDataString(contentType, name, ((bool)val) ? "true" : "false");
                        case SystemDouble:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((double)val).ToString(CultureInfo.InvariantCulture));
                        case SystemSingle:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((float)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt16:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((short)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt32:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((int)val).ToString(CultureInfo.InvariantCulture));
                        case SystemInt64:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((long)val).ToString(CultureInfo.InvariantCulture));
                        case SystemGuid:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((Guid)val).ToString());
                        case SystemDateTime:
                            return (_, val) => new HttpRequestDataString(contentType, name, ((DateTime)val).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        case SystemString:
                            return (_, val) => new HttpRequestDataString(contentType, name, (string)val);
                        default:
                            throw new NotImplementedException("cannot handle type:" + type.FullName + ":" + contentType);
                    }
                case "application/json":
                    return (_, val) => new HttpRequestDataBinary(contentType, name, JsonHelper.Serialize(val, type));
                default:
                    throw new NotImplementedException("cannot handle content type:" + contentType);
            }
        }

       /// <summary>
        /// The name of the request data.
        /// </summary>
        /// <param name="name"></param>
        public HttpRequestData(string contentType, string name)
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