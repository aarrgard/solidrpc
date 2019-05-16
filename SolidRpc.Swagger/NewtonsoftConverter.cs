using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger
{
    public class NewtonsoftConverter<T> : JsonConverter
    {
        public NewtonsoftConverter()
        {
            ReadPropertyHandler = new ConcurrentDictionary<string, Action<JsonReader, object, JsonSerializer>>();
            DynamicType = GetDynamicType(typeof(T));
            PropertyWriters = CreatePropertyWriters();
        }

        private IEnumerable<Action<JsonWriter, object, JsonSerializer>> CreatePropertyWriters()
        {
            var pWriters = new List<Action<JsonWriter, object, JsonSerializer>>();
            var props = typeof(T)
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.GetCustomAttribute<DataMemberAttribute>() != null)
                .ToList();

            foreach(var prop in props)
            {
                var attr = prop.GetCustomAttribute<DataMemberAttribute>();
                var writeMethod = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(WriteProperty))
                    .MakeGenericMethod(prop.PropertyType);
                pWriters.Add((writer, o, serializer) =>
                {
                    writeMethod.Invoke(this, new[] { attr, prop, writer, o, serializer });
                });
            }

            //
            // handle dynamic attributes
            //
            if(DynamicType != null)
            {
                var writeMethod = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(WriteDictionaryData))
                    .MakeGenericMethod(DynamicType);
                pWriters.Add((writer, o, serializer) =>
                {
                    writeMethod.Invoke(this, new[] { writer, o, serializer });
                });
            }
            return pWriters;
        }

        private void WriteDictionaryData<Tp>(JsonWriter writer, object o, JsonSerializer serializer)
        {
            var dict = (IDictionary<string, Tp>)o;
            foreach(var p in dict)
            {
                writer.WritePropertyName(p.Key);
                serializer.Serialize(writer, p.Value);
            }
        }

        private void WriteProperty<Tp>(DataMemberAttribute attr, PropertyInfo prop, JsonWriter writer, object o, JsonSerializer serializer)
        {
            var val = (Tp)prop.GetValue(o);
            if(!attr.EmitDefaultValue && Equals(val, default(Tp)))
            {
                return;
            }
            writer.WritePropertyName(attr.Name);
            serializer.Serialize(writer, val);
        }

        private static Type GetDynamicType(Type t)
        {
            foreach(var i in t.GetInterfaces())
            {
                if(i.IsGenericType)
                {
                    if(typeof(IDictionary<,>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                    {
                        return i.GetGenericArguments()[1];
                    }
                }
            }
            return null;
        }

        private ConcurrentDictionary<string, Action<JsonReader, object, JsonSerializer>> ReadPropertyHandler { get; }
        public IEnumerable<Action<JsonWriter, object, JsonSerializer>> PropertyWriters { get; }
        private Type DynamicType { get; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Not start of object");
            }
            reader.Read();
            if(existingValue == null)
            {
                existingValue = Activator.CreateInstance<T>();
            }
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;
                reader.Read();
                ReadPropertyHandler.GetOrAdd(propertyName, CreateReadPropertyHandler).Invoke(reader, existingValue, serializer);
                reader.Read();
            }
            if (reader.TokenType != JsonToken.EndObject)
            {
                throw new Exception("Not end of object");
            }
            //reader.Read();
            return existingValue;
        }

        private Action<JsonReader, object, JsonSerializer> CreateReadPropertyHandler(string propertyName)
        {
            var prop = typeof(T)
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.GetCustomAttribute<DataMemberAttribute>() != null)
                .Where(o => string.Equals(o.GetCustomAttribute<DataMemberAttribute>().Name, propertyName))
                .SingleOrDefault();

            if(prop != null)
            {
                return (r, o, s) =>
                {
                    prop.SetValue(o, s.Deserialize(r, prop.PropertyType));
                };
            }
            if(DynamicType != null)
            {
                var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(ReadDictionaryData))
                    .MakeGenericMethod(DynamicType);
                return (r, o, s) =>
                {
                    m.Invoke(this, new[] { propertyName, r, o, s });
                };
            }
            throw new NotImplementedException($"Cannot handle property:{typeof(T).FullName}.{propertyName}");
        }

        private void ReadDictionaryData<Tp>(string propertyName, JsonReader r, object o, JsonSerializer s) 
        {
            var dict = (IDictionary<string, Tp>) o;
            dict[propertyName] = s.Deserialize<Tp>(r);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach(var pWriter in PropertyWriters)
            {
                pWriter(writer, value, serializer);
            }
            writer.WriteEndObject();
        }
    }
}
