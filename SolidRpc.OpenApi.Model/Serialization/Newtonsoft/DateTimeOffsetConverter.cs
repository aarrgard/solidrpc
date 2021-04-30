using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SolidRpc.Abstractions.Serialization;
using System;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class DateTimeOffsetConverter : JsonConverter
    {
        public DateTimeOffsetConverter(SerializerSettings serializerSettings)
        {
            TimeZoneTicks = DateTimeOffset.Now.Offset.Ticks;
            SerializerSettings = serializerSettings;
        }

        private long TimeZoneTicks { get; }
        private SerializerSettings SerializerSettings { get; }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return DateTimeOffset.MinValue;
                case JsonToken.Date:
                    var dt = (DateTime)reader.Value;
                    if(dt.Ticks < TimeZoneTicks)
                    {
                        return DateTimeOffset.MinValue;
                    }
                    switch(dt.Kind)
                    {
                        case DateTimeKind.Utc:
                        case DateTimeKind.Local:
                            return new DateTimeOffset(dt);
                        case DateTimeKind.Unspecified:
                            return new DateTimeOffset(dt, SerializerSettings.DefaultTimeZone.GetUtcOffset(dt));
                        default:
                            throw new Exception("Cannot handle datetime kind.");
                    }
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
