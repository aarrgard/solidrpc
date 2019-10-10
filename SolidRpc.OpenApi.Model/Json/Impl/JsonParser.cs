using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a json parser.
    /// </summary>
    public class JsonParser
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public JsonParser()
        {
        }

        /// <summary>
        /// Parses the supplied text as json.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public T Parse<T>(string text) where T : IJsonStruct
        {
            using (var tr = new StringReader(text))
            {
                return Parse<T>(tr);
            }
        }

        /// <summary>
        /// Parses the supplied text as json.
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public T Parse<T>(TextReader tr) where T : IJsonStruct
        {
            using (var jr = new JsonTextReader(tr))
            {
                return Parse<T>(jr);
            }
        }

        /// <summary>
        /// Parses the content in the reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jr"></param>
        /// <returns></returns>
        public T Parse<T>(JsonTextReader jr) where T : IJsonStruct
        {
            if(!jr.Read())
            {
                return default(T);
            }
            return Parse<T>(jr, null);
        }

        private T Parse<T>(JsonTextReader jr, IJsonStruct parent) where T : IJsonStruct
        {
            IJsonStruct s;
            switch(jr.TokenType)
            {
                case JsonToken.Null:
                    s = null;
                    break;
                case JsonToken.String:
                    s = new JsonString(parent) { String = (string)jr.Value };
                    break;
                case JsonToken.Boolean:
                    s = new JsonBoolean(parent) { Boolean = (bool)jr.Value };
                    break;
                case JsonToken.Integer:
                    s = new JsonInteger(parent) { Integer = (long)jr.Value };
                    break;
                case JsonToken.Float:
                    s = new JsonFloat(parent) { Float = (double)jr.Value };
                    break;
                case JsonToken.StartArray:
                    s = ParseArray<IJsonStruct>(jr, parent);
                    break;
                case JsonToken.StartObject:
                    s = ParseObject(jr, parent);
                    break;
                default:
                    throw new Exception("Cannot handle token type:" + jr.TokenType);
            }
            return (T)(IJsonStruct)s;
        }

        private JsonObject ParseObject(JsonTextReader jr, IJsonStruct parent)
        {
            if (jr.TokenType != JsonToken.StartObject) throw new Exception("Not an object");
            var obj = new JsonObject(parent);
            if (!jr.Read()) throw new Exception("Failed to read object element.");
            while (jr.TokenType != JsonToken.EndObject)
            {
                if(jr.TokenType != JsonToken.PropertyName) throw new Exception("Failed to read object element.");
                var name = (string)jr.Value;
                if (!jr.Read()) throw new Exception("Failed to read object element.");
                obj[name] = Parse<IJsonStruct>(jr, obj);
                if (!jr.Read()) throw new Exception("Failed to read object element.");
            }
            return obj;
        }

        private JsonArray<T> ParseArray<T>(JsonTextReader jr, IJsonStruct parent) where T : IJsonStruct
        {
            if (jr.TokenType != JsonToken.StartArray) throw new Exception("Not an array");
            var arr = new JsonArray<T>(parent);
            if (!jr.Read()) throw new Exception("Failed to read array element.");
            while (jr.TokenType != JsonToken.EndArray)
            {
                arr.AddStruct(Parse<T>(jr, arr));
                if (!jr.Read()) throw new Exception("Failed to read array element.");
            }
            return arr;
        }
    }
}
