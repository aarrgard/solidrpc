using Newtonsoft.Json;
using SolidRpc.OpenApi.Model.Serialization.Newtonsoft;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Helper class to seralize/deserialize json
    /// </summary>
    public class JsonHelper
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        private static ConcurrentDictionary<Type, Func<object, object>> s_makeArray = new ConcurrentDictionary<Type, Func<object, object>>();
        private static readonly JsonSerializer s_serializer = new JsonSerializer()
        {
            ContractResolver = NewtonsoftContractResolver.Instance,
        };

        /// <summary>
        /// Deserializes the supplied stream into an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream, Encoding encoding = null)
        {
            return (T)Deserialize(stream, typeof(T), encoding);
        }

        /// <summary>
        /// Deserializes the supplied stream into an object
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="objectType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object Deserialize(Stream stream, Type objectType, Encoding encoding = null)
        {
            StreamReader sr;
            if (encoding == null)
            {
                sr = new StreamReader(stream);
            }
            else
            {
                sr = new StreamReader(stream, encoding);
            }
            using (sr)
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return s_serializer.Deserialize(reader, objectType);
                }
            }
        }

        /// <summary>
        /// Serializes supplied object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static Stream Serialize(object obj, Type objectType)
        {
            using (var ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    Serialize(sw, obj, objectType);
                }
                return new MemoryStream(ms.ToArray());
            }

        }

        /// <summary>
        /// Serializes supplied object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static void Serialize(TextWriter sw, object obj, Type objectType)
        {
            // convert enumerable types into arrays.
            // this is to create concrete object if a linq enum is supplied.
            obj = s_makeArray.GetOrAdd(objectType, _ =>
            {
                if (_.IsGenericType && _.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return (Func<object, object>)typeof(JsonHelper).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                        .Single(o => o.Name == nameof(MakeArray))
                        .MakeGenericMethod(objectType.GetGenericArguments()[0])
                        .CreateDelegate(typeof(Func<object, object>));
                }
                return o => o;
            })(obj);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                s_serializer.Serialize(jsonWriter, obj, objectType);
            }
        }

        private static object MakeArray<T>(object e)
        {
            return ((IEnumerable<T>)e).ToArray();
        }
    }
}
