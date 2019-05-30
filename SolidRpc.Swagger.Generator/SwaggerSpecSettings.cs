using System;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// Settings for generating a swagger spec.
    /// </summary>
    public class SwaggerSpecSettings
    {
        /// <summary>
        /// Constructs a new settings instance
        /// </summary>
        public SwaggerSpecSettings()
        {
            SwaggerVersion = "2.0";
            MapPath = s => $"/{s.Replace('.', '/')}";
        }

        /// <summary>
        /// The swagger version to generate
        /// </summary>
        public string SwaggerVersion { get; set; }

        /// <summary>
        /// The path wher the code resides. All the .cs files in this
        /// folder and the subfolders will be analyzed
        /// </summary>
        public string CodePath { get; set; }

        /// <summary>
        /// The swagger file that we are generating.
        /// </summary>
        public string SwaggerFile { get; set; }

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

        /// <summary>
        /// The base path for the service
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// The function that maps the method name to a path.
        /// </summary>
        public Func<string, string> MapPath { get; set; }

    }
}