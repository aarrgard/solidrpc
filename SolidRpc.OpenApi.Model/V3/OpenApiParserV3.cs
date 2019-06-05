using System;

namespace SolidRpc.OpenApi.Model.V3
{
    /// <summary>
    /// Base class that we can use to parse and write swagger specs.
    /// </summary>
    public class OpenApiParserV3 : OpenApiParser<OpenAPIObject>
    {
        protected override bool CheckVersion(OpenAPIObject res)
        {
            return res.Openapi?.StartsWith("3.") ?? false;
        }
    }
}
