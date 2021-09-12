using System;

namespace SolidRpc.Security
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

        /// <summary>
        /// The path to bind.
        /// </summary>
        public string Path { get; set; }
    }
}
