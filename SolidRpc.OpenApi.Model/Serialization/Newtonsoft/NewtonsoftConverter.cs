using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    /// <summary>
    /// A converter for converting json into structured elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NewtonsoftConverter<T> : JsonConverter
    {
        /// <summary>
        /// constructs a new instance
        /// </summary>
        public NewtonsoftConverter()
        {
            ReadPropertyHandler = new ConcurrentDictionary<string, Action<JsonReader, object, JsonSerializer>>();
            DynamicType = GetDynamicType(typeof(T));
            PropertyWriters = CreatePropertyWriters();
            Constructor = CreateConstructor();
        }

        private Action<object, object> CreateSetParent(Type type)
        {
            if (typeof(ModelBase).IsAssignableFrom(typeof(T)))
            {
                if (typeof(ModelBase).IsAssignableFrom(type))
                {
                    return (v, p) => ((ModelBase)v).SetParent((ModelBase)p);
                }
                if (typeof(IEnumerable<ModelBase>).IsAssignableFrom(type))
                {
                    return (v, p) =>
                    {
                        ((IEnumerable<ModelBase>)v).ToList().ForEach(o => o.SetParent((ModelBase)p));
                    };
                }
                return (v, p) => { };
            }
            return (v, p) => { };
        }

        private Func<T> CreateConstructor()
        {
            if(typeof(ModelBase).IsAssignableFrom(typeof(T)))
            {
                return () => (T)Activator.CreateInstance(typeof(T), new object[] { null });
            }
            else
            {
                return () => (T)Activator.CreateInstance(typeof(T));
            }
        }

        private IEnumerable<Action<JsonWriter, object, JsonSerializer>> CreatePropertyWriters()
        {
            var pWriters = new List<Action<JsonWriter, object, JsonSerializer>>();

            foreach(var prop in GetProperties())
            {
                var attr = prop.GetCustomAttribute<DataMemberAttribute>();
                if(attr == null)
                {
                    attr = new DataMemberAttribute()
                    {
                        Name = prop.Name
                    };
                }
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

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return typeof(T)
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.CanRead)
                .Where(o => o.GetMethod.GetParameters().Length == 0)
                .Where(o => o.CanWrite)
                .Where(o => o.SetMethod.GetParameters().Length == 1)
                .ToList();
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
        private IEnumerable<Action<JsonWriter, object, JsonSerializer>> PropertyWriters { get; }
        private Func<T> Constructor { get; }
        private Type DynamicType { get; }

        /// <summary>
        /// returns true if this converter can convert supplied object type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Not start of object");
            }
            reader.Read();
            if(existingValue == null)
            {
                existingValue = Constructor();
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

        private object Deserialize<Tp>(JsonReader r, object parent, JsonSerializer s, Action<object, object> setParent)
        {
            try
            {
                object val = s.Deserialize<Tp>(r);
                setParent(val, parent);
                return val;
            }
            catch(Exception e)
            {
                throw e;
            }
       }

        private Action<JsonReader, object, JsonSerializer> CreateReadPropertyHandler(string propertyName)
        {
            var props = GetProperties()
                .Where(o => {
                    var propName = o.GetCustomAttribute<DataMemberAttribute>()?.Name ?? o.Name;
                    return string.Equals(propName, propertyName, StringComparison.InvariantCultureIgnoreCase);
                }).ToList();

            if(props.Count == 1)
            {
                var prop = props.First();
                var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(Deserialize))
                    .MakeGenericMethod(prop.PropertyType);
                var sp = CreateSetParent(prop.PropertyType);
                return (r, o, s) =>
                {
                    var val = m.Invoke(this, new[] { r, o, s, sp});
                    prop.SetValue(o, val);
                };
            }

            if(DynamicType != null)
            {
                var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(ReadDictionaryData))
                    .MakeGenericMethod(DynamicType);
                var sp = CreateSetParent(DynamicType);
                return (r, o, s) =>
                {
                    m.Invoke(this, new[] { propertyName, r, o, s, sp });
                };
            }

            throw new NotImplementedException($"Cannot handle property:{typeof(T).FullName}.{propertyName} - found {props.Count} props.");
        }

        private void ReadDictionaryData<Tp>(string propertyName, JsonReader r, object o, JsonSerializer s, Action<object, object> setParent) 
        {
            var dict = (IDictionary<string, Tp>) o;
            dict[propertyName] = (Tp)Deserialize<Tp>(r, o, s, setParent);
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
