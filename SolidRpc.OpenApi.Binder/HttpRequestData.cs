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

        public static Func<object, IEnumerable<HttpRequestData>> CreateBinder(string contentType, string name, Type parameterType, string collectionFormat)
        {
            Func<object, HttpRequestData> subBinder;
            switch(collectionFormat)
            {
                case null:
                    subBinder = CreateBinder(contentType, name, parameterType);
                    return _ => new HttpRequestData[] { subBinder(_) };
                case "multi":
                    var binder = CreateEnumBinder(contentType, name, parameterType);
                    return _ => binder(_);
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

        private static Func<object, IEnumerable<HttpRequestData>> CreateEnumBinder(string contentType, string name, Type type)
        {
            var enumType = GetEnumType(type);
            var m = typeof(HttpRequestData).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(o => o.Name == nameof(CreateEnumBinder))
                    .Where(o => o.GetParameters().Length == 2)
                    .Where(o => o.IsGenericMethod)
                    .Single();
            return (Func<object, IEnumerable<HttpRequestData>>)m.MakeGenericMethod(enumType).Invoke(null, new object[] { contentType, name });
        }

        private static Func<object, IEnumerable<HttpRequestData>> CreateEnumBinder<T>(string contentType, string name)
        {
            var subBinder = CreateBinder(contentType, name, typeof(T));
            return _ => ((IEnumerable<T>)_).Select(o => subBinder(o));
        }

        private static Func<object, HttpRequestData> CreateBinder(string contentType, string name, Type type)
        {
            if(type?.FullName == SystemIOStream)
            {
                contentType = contentType ?? "application/octet-stream";
                return (_) =>
                {
                    var ms = new MemoryStream();
                    ((Stream)_).CopyTo(ms);
                    return new HttpRequestDataBinary(contentType, name, ms.ToArray());
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
                            return (_) => null;
                        case SystemBoolean:
                            return (_) => new HttpRequestDataString(contentType, name, ((bool)_) ? "true" : "false");
                        case SystemDouble:
                            return (_) => new HttpRequestDataString(contentType, name, ((double)_).ToString(CultureInfo.InvariantCulture));
                        case SystemSingle:
                            return (_) => new HttpRequestDataString(contentType, name, ((float)_).ToString(CultureInfo.InvariantCulture));
                        case SystemInt16:
                            return (_) => new HttpRequestDataString(contentType, name, ((short)_).ToString(CultureInfo.InvariantCulture));
                        case SystemInt32:
                            return (_) => new HttpRequestDataString(contentType, name, ((int)_).ToString(CultureInfo.InvariantCulture));
                        case SystemInt64:
                            return (_) => new HttpRequestDataString(contentType, name, ((long)_).ToString(CultureInfo.InvariantCulture));
                        case SystemGuid:
                            return (_) => new HttpRequestDataString(contentType, name, ((Guid)_).ToString());
                        case SystemDateTime:
                            return (_) => new HttpRequestDataString(contentType, name, ((DateTime)_).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                        case SystemString:
                            return (_) => new HttpRequestDataString(contentType, name, (string)_);
                        default:
                            throw new NotImplementedException("cannot handle type:" + type.FullName + ":" + contentType);
                    }
                case "application/json":
                    return (_) => new HttpRequestDataString(contentType, name, JsonConvert.SerializeObject(_));
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
        public string ContentType { get; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; }

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
            switch(ContentType)
            {
                case "text/plain":
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