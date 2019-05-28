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
    }
}