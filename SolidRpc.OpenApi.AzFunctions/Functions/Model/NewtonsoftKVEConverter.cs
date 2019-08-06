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
    public class NewtonsoftKVEConverter<T> : JsonConverter
    {

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public NewtonsoftKVEConverter(Type collectionType)
        {
            CollectionType = collectionType;
            CreateCollection = CreateCollectionFunction(collectionType);
        }

        private Func<ICollection<KeyValuePair<string, T>>> CreateCollectionFunction(Type collectionType)
        {
            if (collectionType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                return () => { return new Dictionary<string, T>(); };
            }
            else if (collectionType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return () => { return new Dictionary<string, T>(); };
            }
            else if (collectionType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return () => { return new List<KeyValuePair<string, T>>(); };
            }
            else if (collectionType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return () => { return new List<KeyValuePair<string, T>>(); };
            }
            else
            {
                throw new Exception("Cannot create collection:"+ collectionType.FullName);
            }
        }

        /// <summary>
        /// The collection type.
        /// </summary>
        public Type CollectionType { get; }

        private Func<ICollection<KeyValuePair<string, T>>> CreateCollection { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<KeyValuePair<string,T>>) == objectType;
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
            var coll = existingValue as ICollection<KeyValuePair<string, T>>;
            if(coll == null)
            {
                coll = CreateCollection();
            }
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;
                reader.Read();
                coll.Add(new KeyValuePair<string, T>(propertyName, serializer.Deserialize<T>(reader)));
                reader.Read();
            }
            if (reader.TokenType != JsonToken.EndObject)
            {
                throw new Exception("Not end of object");
            }
            //reader.Read();
            return coll;
        }

        private Tp Deserialize<Tp>(JsonReader r, object o, JsonSerializer s)
        {
            return s.Deserialize<Tp>(r);
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
            foreach(var e in ((IEnumerable<KeyValuePair<string,T>>) value))
            {
                writer.WritePropertyName(e.Key);
                serializer.Serialize(writer, e.Value);
            }
            writer.WriteEndObject();
        }
    }
}
