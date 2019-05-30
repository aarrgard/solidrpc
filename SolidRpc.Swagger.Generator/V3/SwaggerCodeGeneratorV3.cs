using System;
using SolidRpc.Swagger.Generator.Model.CSharp;
using SolidRpc.Swagger.Model.V3;

namespace SolidRpc.Swagger.Generator.V2
{
    public class SwaggerCodeGeneratorV3 : SwaggerCodeGenerator
    {
        public SwaggerCodeGeneratorV3(OpenAPIObject apiObject, SwaggerCodeSettings codeSettings)
            : base(codeSettings)
        {
            ApiObject = apiObject;
        }

        public OpenAPIObject ApiObject { get; }

        protected override void GenerateCode(ICSharpRepository repo)
        {
            throw new NotImplementedException();
        }
    }
}
