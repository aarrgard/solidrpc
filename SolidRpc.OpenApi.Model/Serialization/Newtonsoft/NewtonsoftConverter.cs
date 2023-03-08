using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    /// <summary>
    /// A converter for converting json into structured elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NewtonsoftConverter<T> : JsonConverter
    {
        private delegate object ReadPropertyHandler(JsonReader r, object val, JsonSerializer s, ref IDictionary<string, string> sp);

        private static IEnumerable<PropertyInfo> s_cachedProperties;

        private class PropertyMetaData
        {
            private Lazy<object> _defaultValue;
            
            public PropertyMetaData(PropertyInfo propertyInfo)
            {
                PropertyInfo = propertyInfo;
                ValueGetter = typeof(NewtonsoftConverter<T>).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(Deserialize))
                    .MakeGenericMethod(PropertyInfo.PropertyType);
                _defaultValue = new Lazy<object>(() =>
                {
                    if (!PropertyInfo.DeclaringType.GetConstructors().Any(o => o.GetParameters().Length == 0))
                    {
                        return null;
                    }

                    return PropertyInfo.GetValue(Activator.CreateInstance(PropertyInfo.DeclaringType));
                });
            }
            public MethodInfo ValueGetter { get; }
            public PropertyInfo PropertyInfo { get; }
            public object DefaultValue => _defaultValue.Value;
        }

        /// <summary>
        /// constructs a new instance
        /// </summary>
        public NewtonsoftConverter()
        {
            ReadPropertyHandlers = new ConcurrentDictionary<string, ReadPropertyHandler>();
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
                if (!prop.DeclaringType.IsAssignableFrom(typeof(T)))
                {
                    continue;
                }
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
            if(s_cachedProperties == null)
            {
                var props = new Dictionary<string, PropertyInfo>();
                typeof(T).Assembly.GetTypes()
                    .Where(o => typeof(T).IsAssignableFrom(o))
                    .SelectMany(o => o.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
                    .Where(o => o.CanRead)
                    .Where(o => o.GetMethod.GetParameters().Length == 0)
                    .Where(o => o.CanWrite)
                    .Where(o => o.SetMethod.GetParameters().Length == 1)
                    .ToList().ForEach(o =>
                    {
                        props[$"{o.DeclaringType.FullName}.{o.Name}"] = o;
                    });

                s_cachedProperties = props.Values.ToList();

            }
            return s_cachedProperties;
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

        private ConcurrentDictionary<string, ReadPropertyHandler> ReadPropertyHandlers { get; }
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

        public override object ReadJson(JsonReader reader, Type objectType, object value, JsonSerializer serializer)
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
            if(value == null)
            {
                value = Constructor();
            }

            //
            // read ordinary properties
            //
            IDictionary<string, string> deferred = null;
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;
                reader.Read();
                value = ReadPropertyHandlers.GetOrAdd(propertyName, CreateReadPropertyHandler).Invoke(reader, value, serializer, ref deferred);
                reader.Read();
            }

            //
            // Handle deferred properties
            //
            if(deferred != null)
            {
                foreach(var x in deferred)
                {
                    var newReader = new JsonTextReader(new StringReader(x.Value));
                    ReadPropertyHandlers[x.Key].Invoke(newReader, value, serializer, ref deferred);
                }
            }
            if (reader.TokenType != JsonToken.EndObject)
            {
                throw new Exception("Not end of object");
            }
            return value;
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

        private ReadPropertyHandler CreateReadPropertyHandler(string propertyName)
        {
            var propertyMetaData = GetProperties()
                .Where(o => {
                    var propName = o.GetCustomAttribute<DataMemberAttribute>()?.Name ?? o.Name;
                    return string.Equals(propName, propertyName, StringComparison.InvariantCultureIgnoreCase);
                })
                .Select(o => new PropertyMetaData(o))
                .ToList();


            if(propertyMetaData.Count == 1)
            {
                var prop = propertyMetaData.Single().PropertyInfo;
                var m = propertyMetaData.Single().ValueGetter;
                var sp = CreateSetParent(prop.PropertyType);
                return (JsonReader r, object o, JsonSerializer s, ref IDictionary<string, string> rp) =>
                {
                    var val = m.Invoke(this, new[] { r, o, s, sp});
                    try
                    {
                        prop.SetValue(o, val);
                        return o;
                    } 
                    catch(TargetException e)
                    {
                        var newo = Activator.CreateInstance(prop.DeclaringType);
                        foreach(var p in o.GetType().GetProperties())
                        {
                            p.SetValue(newo, p.GetValue(o));
                        }
                        prop.SetValue(newo, val);
                        return newo;
                    }
                };
            }
            if (propertyMetaData.Count > 1)
            {
                bool upcast = propertyMetaData.Select(o => o.DefaultValue).Where(o => o != null).Distinct().Count() > 0;
                return (JsonReader r, object o, JsonSerializer s, ref IDictionary<string, string> rp) =>
                {
                    var pmd = propertyMetaData.FirstOrDefault(x => x.PropertyInfo.DeclaringType.IsAssignableFrom(o.GetType()));
                    if(pmd == null)
                    {
                        rp = rp ?? new Dictionary<string, string>();
                        var sb = new StringBuilder();
                        var res = SkipNode(r,o,s,sb);
                        rp[propertyName] = sb.ToString();
                        return res;
                    }
                    var sp = CreateSetParent(pmd.PropertyInfo.PropertyType);
                    var val = pmd.ValueGetter.Invoke(this, new[] { r, o, s, sp });
                    pmd.PropertyInfo.SetValue(o, val);

                    // check if we need to upcast
                    if(upcast)
                    {
                        var newMeta = propertyMetaData.Where(x => x.DefaultValue != null).FirstOrDefault(x => x.DefaultValue.Equals(val));
                        if (newMeta != null)
                        {
                            var newType = newMeta.PropertyInfo.DeclaringType;
                            var newo = Activator.CreateInstance(newType);
                            foreach (var p in o.GetType().GetProperties())
                            {
                                p.SetValue(newo, p.GetValue(o));
                            }
                            o = newo;
                        }
                    }

                    return o;
                };
            }
            if (DynamicType != null)
            {
                var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(o => o.Name == nameof(ReadDictionaryData))
                    .MakeGenericMethod(DynamicType);
                var sp = CreateSetParent(DynamicType);
                return (JsonReader r, object o, JsonSerializer s, ref IDictionary<string, string> rp) =>
                {
                    m.Invoke(this, new[] { propertyName, r, o, s, sp });
                    return o;
                };
            }

            return (JsonReader r, object o, JsonSerializer s, ref IDictionary<string, string> rp) => { return SkipNode(r, o, s, null); };
        }

        private object SkipNode(JsonReader r, object o, JsonSerializer s, StringBuilder sb)
        {
            switch(r.TokenType)
            {
                case JsonToken.StartObject:
                    SkipNodeObject(r, o, s, sb);
                    break;
                case JsonToken.StartArray:
                    SkipNodeArray(r, o, s, sb);
                    break;
                case JsonToken.String:
                case JsonToken.Date:
                    sb?.Append($"\"{r.Value}\"");
                    break;
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                    sb?.Append(r.Value);
                    break;
                case JsonToken.Null:
                    sb?.Append("null");
                    break;
                case JsonToken.Comment:
                default:
                    break;
            }
            return o;
        }

        private void SkipNodeObject(JsonReader r, object o, JsonSerializer s, StringBuilder sb)
        {
            if (r.TokenType != JsonToken.StartObject) throw new Exception("Not a start of object");
            sb?.Append('{');
            if (!r.Read()) throw new Exception("Failed to read");
            while (r.TokenType != JsonToken.EndObject)
            {
                if (r.TokenType != JsonToken.PropertyName) throw new Exception("Not a property name:" + r.TokenType);
                sb?.Append($"\"{r.Value}\":");
                if (!r.Read()) throw new Exception("Failed to read");
                SkipNode(r, o, s, sb);
                if (!r.Read()) throw new Exception("Failed to read");
            }
            sb?.Append('}');
        }

        private void SkipNodeArray(JsonReader r, object o, JsonSerializer s, StringBuilder sb)
        {
            if (r.TokenType != JsonToken.StartArray) throw new Exception("Not a start of object");
            sb?.Append('[');
            if (!r.Read()) throw new Exception("Failed to read");
            while (r.TokenType != JsonToken.EndArray)
            {
                SkipNode(r, o, s,sb);
                if (!r.Read()) throw new Exception("Failed to read");
                if(r.TokenType != JsonToken.EndArray)
                {
                    sb?.Append(',');
                }
            }
            sb?.Append(']');
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
