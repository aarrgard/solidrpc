using Newtonsoft.Json;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Swagger.Model.V3;
using System.IO;

namespace SolidRpc.Swagger.Model
{
    public class SwaggerParser
    {
        private static SwaggerParserV2 v2Parser = new SwaggerParserV2();
        private static SwaggerParserV3 v3Parser = new SwaggerParserV3();

        /// <summary>
        /// Tries to parse the supplied json as a V2 then V3 spec
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ISwaggerSpec ParseSwaggerSpec(string json)
        {
            var res = (ISwaggerSpec) v2Parser.ParseSwaggerDoc(json);
            if(res == null)
            {
                res = v3Parser.ParseSwaggerDoc(json);
            }
            return res;
        } 
    }

    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public abstract class SwaggerParser<T> : SwaggerParser where T : ISwaggerSpec
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
        public string WriteSwaggerDoc(T swaggerSpec)
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
        private void WriteSwaggerDoc(TextWriter sw, T swaggerSpec)
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
        public T ParseSwaggerDoc(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                return ParseSwaggerDoc(sr);
            }
        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public T ParseSwaggerDoc(string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                return ParseSwaggerDoc(sr);
            }

        }

        /// <summary>
        /// Parses the supplied stream into a swagger doc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sr"></param>
        /// <returns></returns>
        public T ParseSwaggerDoc(TextReader sr)
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.Create(s_settings);
                var res = serializer.Deserialize<T>(reader);
                if (!CheckVersion(res)) return default(T);
                return res;
            }
        }

        protected abstract bool CheckVersion(T res);
    }
}
