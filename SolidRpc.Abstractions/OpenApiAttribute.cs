using System;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Configures the openapi
    /// </summary>
    public class OpenApiAttribute : Attribute
    {
        /// <summary>
        /// Where should the parameter be put.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The verbs to use.
        /// </summary>
        public string[] Verbs { get; set; }
    }
}
