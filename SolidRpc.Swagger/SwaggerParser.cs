using Newtonsoft.Json;
using SolidRpc.Swagger.V2;
using System;
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
            ContractResolver = NewtonsoftContractResolver.Instance
        };

        /// <summary>
        /// Writes the supplied specification to string
        /// </summary>
        /// <param name="swaggerSpec"></param>
        /// <returns></returns>
        public static string WriteSwaggerDoc(SwaggerObject swaggerSpec)
        {
            var sw = new StringWriter();
            WriteSwaggerDoc(sw, swaggerSpec);
            return sw.ToString();
        }

        /// <summary>
        /// Writes the supplied specification to the writer
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="swaggerSpec"></param>
        private static void WriteSwaggerDoc(TextWriter sw, SwaggerObject swaggerSpec)
        {
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = JsonSerializer.Create(s_settings);
                serializer.Serialize(writer, swaggerSpec);
            }
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ParseSwaggerDoc<T>(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                return ParseSwaggerDoc<T>(sr);
            }
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ParseSwaggerDoc<T>(string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                return ParseSwaggerDoc<T>(sr);
            }

        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static T ParseSwaggerDoc<T>(TextReader sr)
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.Create(s_settings);
                return serializer.Deserialize<T>(reader);
            }
        }
    }
}
