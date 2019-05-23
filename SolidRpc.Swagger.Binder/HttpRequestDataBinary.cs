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
        /// <param name="binaryData"></param>
        public HttpRequestDataBinary(string name, byte[] binaryData) : base(name)
        {
            BinaryData = binaryData;
        }

        /// <summary>
        /// The binary data.
        /// </summary>
        public byte[] BinaryData { get; }

        public override HttpRequestData AppendData(HttpRequestData b)
        {
            throw new System.NotImplementedException();
        }

        public override string GetStringValue(Encoding encoding)
        {
            return GetEncoding(encoding).GetString(BinaryData, 0, BinaryData.Length);
        }
    }
}