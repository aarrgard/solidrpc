using SolidRpc.Swagger.Generator.V2;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Swagger.Model.V3;
using System;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// Code generator to create interfaces and classes from a swagger specification.
    /// </summary>
    public abstract class SwaggerCodeGenerator
    {
        protected SwaggerCodeGenerator(SwaggerCodeSettings codeSettings)
        {
            CodeSettings = codeSettings;
        }

        public SwaggerCodeSettings CodeSettings { get; }

        /// <summary>
        /// Generates code 
        /// </summary>
        /// <param name="codeSettings"></param>
        public static void GenerateCode(SwaggerCodeSettings codeSettings)
        {
            var model = SwaggerParser.ParseSwaggerSpec(codeSettings.SwaggerSpec);
            if (model is SwaggerObject v2)
            {
                new SwaggerCodeGeneratorV2(v2, codeSettings).GenerateCode();
            }
            else if (model is OpenAPIObject v3)
            {
                new SwaggerCodeGeneratorV3(v3, codeSettings).GenerateCode();
            } else
            {
                throw new Exception("Cannot parse swagger json.");
            }
        }
        protected abstract void GenerateCode();
    }
}
