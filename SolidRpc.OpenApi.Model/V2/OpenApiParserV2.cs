using System;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class OpenApiParserV2 : OpenApiParser<SwaggerObject>
    {
        protected override bool CheckVersion(SwaggerObject res)
        {
            return res.Swagger?.StartsWith("2.") ?? false;
        }
    }
}
