using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public class SolidHttpRequestDataString : SolidHttpRequestData
    {
        /// <summary>
        /// Constructs a new structure representing string data.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="name"></param>
        /// <param name="stringData"></param>
        public SolidHttpRequestDataString(string contentType, string name, string stringData) : base(contentType, name)
        {
            StringData = stringData;
        }

        /// <summary>
        /// The string data.
        /// </summary>
        public string StringData { get; }

        public override byte[] GetBinaryValue()
        {
            return GetEncoding().GetBytes(StringData);
        }

        public override Stream GetStreamValue()
        {
            return new MemoryStream(GetEncoding().GetBytes(StringData));
        }

        public override string GetStringValue()
        {
            return StringData;
        }
    }
}