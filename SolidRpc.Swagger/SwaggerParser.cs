using Newtonsoft.Json;
using System.IO;

namespace SolidRpc.Swagger
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class SwaggerParser
    {
        private static JsonSerializerSettings s_settings = new JsonSerializerSettings()
        {
            ContractResolver = NewtonsoftContractResolver.Instance,
            TypeNameHandling = TypeNameHandling
        };

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ParseSwaggerDoc<T>(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.Create(s_settings);

                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                return serializer.Deserialize<T>(reader);
            }
        }
    }
}
