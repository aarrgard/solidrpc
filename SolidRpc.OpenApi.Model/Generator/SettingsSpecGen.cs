using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Generator
{
    /// <summary>
    /// Settings for generating a swagger file from code.
    /// </summary>
    public class SettingsSpecGen : SettingsGen
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SettingsSpecGen()
        {
            OpenApiVersion = "2.0";
        }

        /// <summary>
        /// The openapi/swagger version to generate
        /// </summary>
        public string OpenApiVersion { get; set; }

        /// <summary>
        /// The base path to put in the openapi/swagger spec.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// The license name
        /// </summary>
        public string LicenseName { get; set; }

        /// <summary>
        /// The license url.
        /// </summary>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// The version of the swagger spec
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The tile in the document
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description to set in the document
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The contact email
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// The contact name
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// The contact url
        /// </summary>
        public string ContactUrl { get; set; }
    }
}
