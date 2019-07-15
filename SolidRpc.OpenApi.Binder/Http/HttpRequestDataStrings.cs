using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public class HttpRequestDataString : HttpRequestData
    {
        /// <summary>
        /// Constructs a new structure representing string data.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="name"></param>
        /// <param name="stringData"></param>
        public HttpRequestDataString(string contentType, string name, string stringData) : base(contentType, name)
        {
            StringData = stringData;
        }

        /// <summary>
        /// The string data.
        /// </summary>
        public string StringData { get; }

        public override Stream GetBinaryValue(Encoding encoding = null)
        {
            return new MemoryStream(GetEncoding(encoding).GetBytes(StringData));
        }

        public override string GetStringValue(Encoding encoding = null)
        {
            return StringData;
        }
    }
}