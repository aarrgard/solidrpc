using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// The settings for generating code from a swagger specification.
    /// </summary>
    public class SwaggerCodeSettings
    {
        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// The namespace to add to all the generated classes.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// The output path. May be a folder or zip.
        /// </summary>
        public string OutputPath { get; set; }
    }
}
