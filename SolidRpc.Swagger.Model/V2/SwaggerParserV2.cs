using System;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class SwaggerParserV2 : SwaggerParser<SwaggerObject>
    {
        protected override bool CheckVersion(SwaggerObject res)
        {
            return res.Swagger?.StartsWith("2.") ?? false;
        }
    }
}
