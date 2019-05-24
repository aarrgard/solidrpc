using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public abstract class HttpRequestData
    {

        public static readonly IEnumerable<HttpRequestData> EmptyArray = new HttpRequestData[0];

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
            else if (value is bool b)
            {
                return new HttpRequestDataStrings(name, b ? "true" : "false");
            }
            else if (value is short s)
            {
                return new HttpRequestDataStrings(name, s.ToString());
            }
            else if (value is int i)
            {
                return new HttpRequestDataStrings(name, i.ToString());
            }
            else if (value is long l)
            {
                return new HttpRequestDataStrings(name, l.ToString());
            }
            else if (value is Guid g)
            {
                return new HttpRequestDataStrings(name, g.ToString());
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