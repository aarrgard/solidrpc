using System;
using System.Text;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public abstract class HttpRequestData
    {
        /// <summary>
        /// Constructs a new request data instance
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpRequestData Create(string name, object value)
        {
            if (value is string str)
            {
                return new HttpRequestDataStrings(name, str);
            }
            else if (value is string[] strArr)
            {
                return new HttpRequestDataStrings(name, strArr);
            }
            else if (value is byte[] bytes)
            {
                return new HttpRequestDataBinary(name, bytes);
            }
            throw new Exception("Cannot handle value:"+ value?.GetType().FullName);
        }

        public static HttpRequestData operator +(HttpRequestData a, HttpRequestData b)
        {
            if (a == null) return b;
            if (b == null) return a;
            return a.AppendData(b);
        }

        /// <summary>
        /// The name of the request data.
        /// </summary>
        /// <param name="name"></param>
        public HttpRequestData(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected Encoding GetEncoding(Encoding encoding)
        {
            return encoding ?? Encoding.UTF8;
        }

        public abstract HttpRequestData AppendData(HttpRequestData b);

        /// <summary>
        /// Returns the string value using supplied encoding.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public abstract string GetStringValue(Encoding encoding = null);

    }
}