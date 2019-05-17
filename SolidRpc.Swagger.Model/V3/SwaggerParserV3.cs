using System;

namespace SolidRpc.Swagger.Model.V3
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class SwaggerParserV3 : SwaggerParser<OpenAPIObject>
    {
        protected override bool CheckVersion(OpenAPIObject res)
        {
            return res.Openapi?.StartsWith("3.") ?? false;
        }
    }
}
