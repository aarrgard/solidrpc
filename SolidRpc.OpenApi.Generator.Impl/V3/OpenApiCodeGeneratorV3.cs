using System;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.V3;

namespace SolidRpc.OpenApi.Generator.V3

{
    public class OpenApiCodeGeneratorV3 : OpenApiCodeGenerator
    {
        public OpenApiCodeGeneratorV3(OpenAPIObject apiObject, SettingsCodeGen codeSettings)
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
