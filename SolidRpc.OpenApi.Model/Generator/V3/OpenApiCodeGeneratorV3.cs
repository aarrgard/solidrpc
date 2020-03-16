using System;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.V3;

namespace SolidRpc.OpenApi.Model.Generator.V3

{
    /// <summary>
    /// Implements the V3 generator
    /// </summary>
    public class OpenApiCodeGeneratorV3 : OpenApiCodeGenerator
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="apiObject"></param>
        /// <param name="codeSettings"></param>
        public OpenApiCodeGeneratorV3(OpenAPIObject apiObject, SettingsCodeGen codeSettings)
            : base(codeSettings)
        {
            ApiObject = apiObject;
        }

        /// <summary>
        /// The api object
        /// </summary>
        public OpenAPIObject ApiObject { get; }

        /// <summary>
        /// Generates the code
        /// </summary>
        /// <param name="repo"></param>
        protected override void GenerateCode(ICSharpRepository repo)
        {
            throw new NotImplementedException();
        }
    }
}
