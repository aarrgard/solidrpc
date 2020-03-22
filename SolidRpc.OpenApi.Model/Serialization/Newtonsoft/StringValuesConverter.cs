using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class StringValuesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StringValues);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Not start of array!");
            }
            var sv = new StringValues();
            while(reader.TokenType != JsonToken.EndArray && reader.Read())
            {
                var s = (string)reader.Value;
                sv = StringValues.Concat(sv, s);
            }
            if(reader.TokenType != JsonToken.EndArray)
            {
                throw new Exception("Not end of array!");
            }
            return sv;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            ((StringValues)value).ToList().ForEach(o => writer.WriteValue(o));
            writer.WriteEndArray();
        }
    }
}
