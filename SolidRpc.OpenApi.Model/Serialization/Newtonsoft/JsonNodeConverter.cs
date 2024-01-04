using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonNodeConverter 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static bool CanConvert(Type objectType)
        {
            if (objectType.Name != "JsonNode")
            {
                return false;
            }
            if(objectType.GetProperties().Length != 0)
            {
                return false;
            }
            if (!objectType.GetMethods().Any(o => IsCastOp(o, typeof(string), objectType)))
            {
                return false;
            }
            if (!objectType.GetMethods().Any(o => IsCastOp(o, objectType, typeof(string))))
            {
                return false;
            }
            return true;
        }

        public static bool IsCastOp(MethodInfo o, Type returnType, Type argType)
        {
            if (o.Name != "op_Implicit") return false;
            if (o.ReturnType != returnType) return false;
            if (o.GetParameters().Length != 1) return false;
            if (o.GetParameters()[0].ParameterType != argType) return false;
            return true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonNodeConverter<T> : JsonConverter
    {
        private static MethodInfo _jsonNode2String = typeof(T).GetMethods().Single(o => JsonNodeConverter.IsCastOp(o, typeof(string), typeof(T)));
        private static MethodInfo _string2JsonNode = typeof(T).GetMethods().Single(o => JsonNodeConverter.IsCastOp(o, typeof(T), typeof(string)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
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
            var sb = new StringBuilder();
            NewtonsoftConverter.SkipNode(reader, serializer, sb);
            return (T)_string2JsonNode.Invoke(this, new[] { sb.ToString() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string json = (string)_jsonNode2String.Invoke(this, new[] { value });
            
            // check that json is valid
            var reader = new JsonTextReader(new StringReader(json));
            if(!reader.Read())
            {
                throw new Exception("Json node is not well formed.");
            }
            try
            {
                NewtonsoftConverter.SkipNode(reader, serializer, null);
            } 
            catch
            {
                throw new Exception("Json node is not well formed.");
            }
            if (reader.Read())
            {
                throw new Exception("Json node is not well formed.");
            }

            writer.WriteRawValue(json);
        }

    }

}
