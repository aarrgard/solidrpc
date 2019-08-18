using Newtonsoft.Json;
using SolidRpc.OpenApi.Model;
using System;
using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    public class JsonHelper
    {
        private static readonly JsonSerializer s_serializer = new JsonSerializer()
        {
            ContractResolver = NewtonsoftContractResolver.Instance,
        };

        public static T Deserialize<T>(Stream stream, Encoding encoding = null)
        {
            return (T)Deserialize(stream, typeof(T), encoding);
        }

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

        public static Stream Serialize(object resp, Type objectType)
        {
            using (var ms = new MemoryStream())
            {
                var enc = Encoding.UTF8;
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                    {
                        s_serializer.Serialize(jsonWriter, resp, objectType);
                    }
                }
                return new MemoryStream(ms.ToArray());
            }
        }
    }
}
