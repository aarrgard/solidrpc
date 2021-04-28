using System;
using System.IO;
using System.Text;

namespace SolidRpc.Abstractions.Serialization
{
    /// <summary>
    /// Implements logic to create seralizers.
    /// </summary>
    public interface ISerializerFactory
    {
        /// <summary>
        /// Returns the settings that are supplied to the serializers
        /// </summary>
        SerializerSettings SerializerSettings { get; }

        /// <summary>
        /// Gets the serializer for suplied media type.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        ISerializer GetSerializer(string mediaType, Encoding charSet = null);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        /// <returns></returns>
        void SerializeToStream<T>(Stream s, T o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        void DeserializeFromStream<T>(Stream s, out T o, string mediaType = "application/json", Encoding charSet = null);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        /// <returns></returns>
        void SerializeToString<T>(out String s, T o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        void DeserializeFromString<T>(String s, out T o, string mediaType = "application/json", Encoding charSet = null);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        /// <returns></returns>
        void SerializeToString(out String s, Type type, object o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false);

        /// <summary>
        /// Deserializes an object from a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        void DeserializeFromString(String s, Type type, out object o, string mediaType = "application/json", Encoding charSet = null);

        /// <summary>
        /// Serializes supplied object to a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        /// <returns></returns>
        void SerializeToStream(Stream s, Type type, object o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false);

        /// <summary>
        /// Deserializes an object from a string representation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        void DeserializeFromStream(Stream s, Type type, out object o, string mediaType = "application/json", Encoding charSet = null);

        /// <summary>
        /// Writes an object to a file instance type.
        /// </summary>
        /// <param name="fileTypeInstance"></param>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        void SerializeToFileType(object fileTypeInstance, Type type, object o, string mediaType, Encoding charSet = null, bool prettyFormat = false);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileTypeInstance"></param>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="prettyFormat"></param>
        void SerializeToFileType<T>(object fileTypeInstance, T o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false);


        /// <summary>
        /// Writes an object to a file instance type.
        /// </summary>
        /// <param name="fileTypeInstance"></param>
        /// <param name="o"></param>
        void DeserializeFromFileType<T>(object fileTypeInstance, out T o);
    }
}
