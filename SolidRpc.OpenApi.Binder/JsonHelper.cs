using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Binder
{
    public class JsonHelper
    {
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
                    var serializer = JsonSerializer.Create();
                    return serializer.Deserialize(reader, objectType);
                }
            }
        }
    }
}
