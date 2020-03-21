using SolidRpc.Abstractions;
using SolidRpc.OpenApi.Model.V3;

[assembly: SolidRpcAbstractionProvider(typeof(OpenApiParserV3), typeof(OpenApiParserV3))]
namespace SolidRpc.OpenApi.Model.V3
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class OpenApiParserV3 : OpenApiParser<OpenAPIObject>
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="serializerFactory"></param>
        public OpenApiParserV3(OpenApiParser baseParser) : base(baseParser)
        {
        }

        protected override bool CheckVersion(OpenAPIObject res)
        {
            return res.Openapi?.StartsWith("3.") ?? false;
        }
    }
}
