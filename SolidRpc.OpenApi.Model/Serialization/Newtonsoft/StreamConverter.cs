using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class StreamConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Stream).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // read base54 encoded stream and return as memory stream.
            var base64EncodedStream = (string)reader.Value;
            return new MemoryStream(Convert.FromBase64String(base64EncodedStream));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Write stream to memory stream and convert to base64
            var ms = new MemoryStream();
            ((Stream)value).CopyTo(ms);
            writer.WriteValue(Convert.ToBase64String(ms.ToArray()));
        }
    }
}
