using Newtonsoft.Json;
using SolidRpc.OpenApi.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Helper class to seralize/deserialize json
    /// </summary>
    public class JsonHelper
    {
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
            if(encoding == null)
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
            // convert enumerable types into arrays.
            // this is to create concrete object if a linq enum is supplied.
            if(objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var lst = new List<object>();
                foreach(var o in ((IEnumerable)obj))
                {
                    lst.Add(o);
                }
                obj = Array.CreateInstance(objectType.GetGenericArguments()[0], lst.Count);
                lst.CopyTo((object[])obj);
            }
            using (var ms = new MemoryStream())
            {
                var enc = Encoding.UTF8;
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                    {
                        s_serializer.Serialize(jsonWriter, obj, objectType);
                    }
                }
                return new MemoryStream(ms.ToArray());
            }
        }
    }
}
