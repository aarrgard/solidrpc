using SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.OpenApiConfig.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface IOpenApiConfig
    {
        /// <summary>
        /// Proxies the string
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <returns></returns>
        string ProxyStrings(
            [OpenApi(In = "formData")]
            string s1,
            [OpenApi(In = "query")]
            string s2,
            string s3);
    }
}
