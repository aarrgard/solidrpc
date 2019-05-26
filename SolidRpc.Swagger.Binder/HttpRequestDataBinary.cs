using System.IO;
using System.Text;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public class HttpRequestDataBinary : HttpRequestData
    {
        /// <summary>
        /// Constructs a new structure representing binary data.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="name"></param>
        /// <param name="binaryData"></param>
        public HttpRequestDataBinary(string contentType, string name, byte[] binaryData) : base(contentType, name)
        {
            BinaryData = binaryData;
        }

        /// <summary>
        /// The binary data.
        /// </summary>
        public byte[] BinaryData { get; }

        public override Stream GetBinaryValue(Encoding encoding = null)
        {
            return new MemoryStream(BinaryData);
        }

        public override string GetStringValue(Encoding encoding)
        {
            return GetEncoding(encoding).GetString(BinaryData, 0, BinaryData.Length);
        }
    }
}