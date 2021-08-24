using System;

namespace SolidRpc.Tests.Swagger.SpecGen.OAuth2
{
    /// <summary>
    /// Configures the openapi bindings
    /// </summary>
    public class OpenApiAttribute : Attribute
    {
        /// <summary>
        /// the name of the method or function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The verbs we use to access this method.
        /// </summary>
        public string[] Verbs { get; set; }
        
        /// <summary>
        /// The path to the method
        /// </summary>
        public string Path { get; set; }
    }
}
