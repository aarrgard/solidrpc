using System.IO;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// Represents some request data.
    /// </summary>
    public interface IHttpRequestData
    {
        /// <summary>
        /// The content type
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The name of the data
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The string representation
        /// </summary>
        /// <returns></returns>
        string GetStringValue(Encoding encoding = null);

        /// <summary>
        /// Returns the binary value.
        /// </summary>
        /// <returns></returns>
        Stream GetBinaryValue(Encoding encoding = null);

        /// <summary>
        /// Returns the data as supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>();
    }
}