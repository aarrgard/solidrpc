using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Stream).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // read base54 encoded stream and return as memory stream.
            var base64EncodedStream = (string)reader.Value;
            if(base64EncodedStream == null)
            {
                return null;
            }
            return new MemoryStream(Convert.FromBase64String(base64EncodedStream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value == null)
            {
                writer.WriteNull();
                return;
            }
            // Write stream to memory stream and convert to base64
            var ms = new MemoryStream();
            ((Stream)value).CopyTo(ms);
            writer.WriteValue(Convert.ToBase64String(ms.ToArray()));
        }
    }
}
