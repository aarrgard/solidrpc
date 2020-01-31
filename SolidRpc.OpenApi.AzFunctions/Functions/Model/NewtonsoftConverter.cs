using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// The json converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NewtonsoftConverter<T> : JsonConverter
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public NewtonsoftConverter()
        {
            ReadPropertyHandler = new ConcurrentDictionary<string, Action<JsonReader, object, JsonSerializer>>();
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
            return pWriters;
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

        private ConcurrentDictionary<string, Action<JsonReader, object, JsonSerializer>> ReadPropertyHandler { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Action<JsonWriter, object, JsonSerializer>> PropertyWriters { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
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
            if(reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Not start of object");
            }
            reader.Read();
            if(existingValue == null)
            {
                existingValue = Activator.CreateInstance(typeof(T));
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

        private Tp Deserialize<Tp>(JsonReader r, object o, JsonSerializer s)
        {
            return s.Deserialize<Tp>(r);
        }

        private Action<JsonReader, object, JsonSerializer> CreateReadPropertyHandler(string propertyName)
        {
            var props = typeof(T)
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.GetCustomAttribute<DataMemberAttribute>() != null)
                .Where(o => string.Equals(o.GetCustomAttribute<DataMemberAttribute>().Name, propertyName))
                .ToList();

            if(props.Count == 1)
            {
                var prop = props.First();
                var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(Deserialize))
                    .MakeGenericMethod(prop.PropertyType);
                return (r, o, s) =>
                {
                    var val = m.Invoke(this, new[] { r, o, s });
                    prop.SetValue(o, val);
                };
            }
            throw new NotImplementedException($"Cannot handle property:{typeof(T).FullName}.{propertyName} - found {props.Count} props.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
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
