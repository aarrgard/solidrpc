using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public class HttpRequestDataStrings : HttpRequestData
    {
        /// <summary>
        /// Constructs a new structure representing string data.
        /// </summary>
        /// <param name="binaryData"></param>
        public HttpRequestDataStrings(string name, params string[] stringData) : base(name)
        {
            StringData = stringData;
        }

        /// <summary>
        /// Constructs a new structure representing string data.
        /// </summary>
        /// <param name="binaryData"></param>
        public HttpRequestDataStrings(string name, IEnumerable<string> stringData) : base(name)
        {
            StringData = stringData;
        }

        /// <summary>
        /// The string data.
        /// </summary>
        public IEnumerable<string> StringData { get; }

        public override HttpRequestData AppendData(HttpRequestData b)
        {
            throw new System.NotImplementedException();
        }

        public override string GetStringValue(Encoding encoding = null)
        {
            return string.Join(" ", StringData);
        }
    }
}