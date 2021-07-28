using System;

namespace SolidRpc.Tests.Swagger.SpecGen.OpenApiConfig.Services
{
    public class OpenApiAttribute : Attribute
    {
        public string In { get; set; }
    }
}