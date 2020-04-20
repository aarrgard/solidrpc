using Newtonsoft.Json;
using System;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class NullableConverter<T> : JsonConverter where T:struct
    {
        public NullableConverter(JsonConverter valueConverter)
        {
            ValueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
        }

        public JsonConverter ValueConverter { get; }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Nullable<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                default:
                    var value = (T) ValueConverter.ReadJson(reader, objectType, existingValue, serializer);
                    return new Nullable<T>(value);

            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) 
            {
                writer.WriteNull();
                return;
            }

            var nullable = (Nullable<T>)value;
            ValueConverter.WriteJson(writer, nullable.Value, serializer);
        }
    }
}
