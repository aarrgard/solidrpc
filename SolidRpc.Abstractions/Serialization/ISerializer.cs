using System;
using System.IO;
using System.Text;

namespace SolidRpc.Abstractions.Serialization
{
    /// <summary>
    /// Defines the logic to serialize object into a wire format 
    /// such as xml or json.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Returns the content type for this serializer
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The charset for this serializer
        /// </summary>
        Encoding CharSet { get; }

        /// <summary>
        /// Serializes an object of specified type.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="prettyFormat"></param>
        void Serialize(Stream stream, Type type, object o, bool prettyFormat = false);

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="prettyFormat"></param>
        /// <returns></returns>
        void Deserialize(Stream stream, Type type, out object o);
    }
}