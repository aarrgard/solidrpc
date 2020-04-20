using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class DateTimeOffsetConverter : JsonConverter
    {
        public DateTimeOffsetConverter()
        {
            TimeZoneTicks = DateTimeOffset.Now.Offset.Ticks;
        }

        public long TimeZoneTicks { get; }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch(reader.TokenType)
            {
                case JsonToken.Null:
                    return DateTimeOffset.MinValue;
                case JsonToken.Date:
                    var dt = (DateTime)reader.Value;
                    if(dt.Ticks < TimeZoneTicks)
                    {
                        return DateTimeOffset.MinValue;
                    }
                    return new DateTimeOffset(dt);
                default:
                    throw new Exception("Cannot handle token type:"+reader.TokenType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) 
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteValue((DateTimeOffset)value);
        }
    }
}
