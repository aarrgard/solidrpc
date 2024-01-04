using System.IO;

namespace SolidRpc.Tests.Swagger.SpecGen.JsonNodeArgs.Types
{
    /// <summary>
    /// Represents some file data
    /// </summary>
    public class JsonNode
    {
        private readonly string _json;

        private JsonNode(string json) { _json = json; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public static implicit operator JsonNode(string json)
        {
            return new JsonNode(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jn"></param>
        public static implicit operator string(JsonNode jn)
        {
            return jn._json;
        }

    }
}
