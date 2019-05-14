using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.Swagger
{
    public class NewtonsoftContractResolver : DefaultContractResolver
    {
        public static NewtonsoftContractResolver Instance = new NewtonsoftContractResolver();
        private static DynamicBaseConverter s_DynamicBaseConverter = new DynamicBaseConverter();

        public NewtonsoftContractResolver()
        {

        }
        protected override JsonContract CreateContract(Type objectType)
        {
            if (s_DynamicBaseConverter.CanConvert(objectType))
            {
                var contract = new JsonObjectContract(objectType);
                contract.Converter = s_DynamicBaseConverter;
                return contract;
            }
            return base.CreateContract(objectType);
        }

        private class DynamicBaseConverter : JsonConverter
        {
            public DynamicBaseConverter()
            {
            }
            private Type GetValueType(Type objectType)
            {
                var baseType = objectType.BaseType;
                if (baseType == null) return null;
                if (baseType == typeof(object)) return null;
                if (!baseType.IsGenericType) return GetValueType(baseType);
                var genType = objectType.BaseType.GetGenericTypeDefinition();
                if (!typeof(DynamicBase<>).IsAssignableFrom(genType)) return null;
                return baseType.GetGenericArguments()[0];
            }

            public override bool CanConvert(Type objectType)
            {
                var valueType = GetValueType(objectType);
                return valueType != null;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                existingValue = existingValue ?? Activator.CreateInstance(objectType);
                var valueType = GetValueType(objectType);
                return GetType().GetMethod(nameof(ReadJson), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(valueType).Invoke(this, new object[] { reader, existingValue, serializer });
            }
            private object ReadJson<T>(JsonReader reader, object value, JsonSerializer serializer)
            {
                var dict = (IDictionary<string, T>)value;
                if (reader.TokenType != JsonToken.StartObject)
                {
                    throw new Exception("Not start of object");
                }
                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = (string)reader.Value;
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                    {
                        throw new Exception("Not start of object");
                    }
                    var val = (T)serializer.Deserialize(reader, typeof(T));
                    dict[propertyName] = val;
                    if (reader.TokenType != JsonToken.EndObject)
                    {
                        throw new Exception("Not end of object");
                    }
                    reader.Read();
                }
                if (reader.TokenType != JsonToken.EndObject)
                {
                    throw new Exception("Not end of object");
                }
                //reader.Read();
                return value;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var valueType = GetValueType(value.GetType());
                GetType().GetMethod(nameof(WriteJsonInternal), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(valueType).Invoke(this, new object[] { writer, value, serializer });
            }
            private void WriteJsonInternal<T>(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                var dict = (IDictionary<string, T>)value;
                foreach (var kv in dict)
                {
                    writer.WritePropertyName(kv.Key);
                    serializer.Serialize(writer, kv.Value);
                }
                writer.WriteEndObject();
            }
        }
    }
}
