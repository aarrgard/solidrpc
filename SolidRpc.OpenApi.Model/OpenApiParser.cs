using Newtonsoft.Json;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System.IO;

namespace SolidRpc.OpenApi.Model
{
    public class OpenApiParser
    {
        private static OpenApiParserV2 v2Parser = new OpenApiParserV2();
        private static OpenApiParserV3 v3Parser = new OpenApiParserV3();

        /// <summary>
        /// Tries to parse the supplied json as a V2 then V3 spec
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IOpenApiSpec ParseOpenApiSpec(string json)
        {
            var res = (IOpenApiSpec) v2Parser.ParseSwaggerDoc(json);
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
    public abstract class OpenApiParser<T> : OpenApiParser where T : IOpenApiSpec
    {
        private static readonly JsonSerializerSettings s_settings = new JsonSerializerSettings()
        {
            ContractResolver = NewtonsoftContractResolver.Instance
        };

        /// <summary>
        /// Writes the supplied specification to string
        /// </summary>
        /// <param name="swaggerSpec"></param>
        /// <returns></returns>
        public static string WriteSwaggerDoc(T swaggerSpec)
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
        private static void WriteSwaggerDoc(TextWriter sw, T swaggerSpec)
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
                return ParseSwaggerDoc(sr.ReadToEnd());
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
        private T ParseSwaggerDoc(TextReader sr)
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
