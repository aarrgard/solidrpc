using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Binder.Http
{
    /// <summary>
    /// Represents some HttpRequest data
    /// </summary>
    public class SolidHttpRequestDataBinary : SolidHttpRequestData
    {
        /// <summary>
        /// Constructs a new structure representing binary data.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="charset"></param>
        /// <param name="name"></param>
        /// <param name="binaryData"></param>
        public SolidHttpRequestDataBinary(string contentType, string charset, string name, byte[] binaryData) : base(contentType, name)
        {
            BinaryData = binaryData;
            Encoding = (charset == null) ? null : Encoding.GetEncoding(charset);
        }
        /// <summary>
        /// Constructs a new structure representing binary data.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="charset"></param>
        /// <param name="name"></param>
        /// <param name="binaryData"></param>
        public SolidHttpRequestDataBinary(string contentType, string charset, string name, Stream stream) : base(contentType, name)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            BinaryData = ms.ToArray();
            Encoding = (charset == null) ? null : Encoding.GetEncoding(charset);
        }

        /// <summary>
        /// The binary data.
        /// </summary>
        public byte[] BinaryData { get; private set; }

        public override Stream GetBinaryValue()
        {
            return new MemoryStream(BinaryData);
        }

        public override string GetStringValue()
        {
            return GetEncoding().GetString(BinaryData, 0, BinaryData.Length);
        }

        /// <summary>
        /// Sets the content type.
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            ContentType = contentType;
        }

        /// <summary>
        /// Sets the filename
        /// </summary>
        /// <param name="charSet"></param>
        public void SetCharSet(string charSet)
        {
            if(string.IsNullOrEmpty(charSet))
            {
                Encoding = null;
            }
            else
            {
                Encoding = Encoding.GetEncoding(charSet);
            }
        }

        /// <summary>
        /// Sets the filename
        /// </summary>
        /// <param name="filename"></param>
        public void SetFilename(string filename)
        {
            Filename = filename;
        }

        /// <summary>
        /// Sets the ETag
        /// </summary>
        /// <param name="v"></param>
        public void SetETag(string eTag)
        {
            ETag = eTag;
        }

        /// <summary>
        /// Sets the binary data.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetBinaryData(string name, Stream value)
        {
            Name = name;
            var ms = new MemoryStream();
            value.CopyTo(ms);
            BinaryData = ms.ToArray();
        }
    }
}