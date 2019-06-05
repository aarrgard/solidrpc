using System;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Model.V3;

namespace SolidRpc.OpenApi.Generator.V2
{
    public class OpenApiCodeGeneratorV3 : OpenApiCodeGenerator
    {
        public OpenApiCodeGeneratorV3(OpenAPIObject apiObject, OpenApiCodeSettings codeSettings)
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
