using SolidRpc.Abstractions;
using SolidRpc.OpenApi.Model.V2;

[assembly: SolidRpcService(typeof(OpenApiParserV2), typeof(OpenApiParserV2))]
namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class OpenApiParserV2 : OpenApiParser<SwaggerObject>
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="baseParser"></param>
        public OpenApiParserV2(OpenApiParser baseParser) : base(baseParser)
        {
        }

        protected override bool CheckVersion(SwaggerObject res)
        {
            return res.Swagger?.StartsWith("2.") ?? false;
        }
    }
}
