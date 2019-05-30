using System;
using SolidRpc.Swagger.Generator.Model.CSharp;

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
            TypeNamespace = "Types";
            ServiceNamespace = "Services";

            MapPath = s => $"/{s.Replace('.', '/')}";
            TypeDefinitionNameMapper = c => {
                var name = c.FullName;
                if (name.StartsWith($"{ProjectNamespace}."))
                {
                    name = name.Substring(ProjectNamespace.Length + 1);
                }
                if (name.StartsWith($"{CodeNamespace}."))
                {
                    name = name.Substring(CodeNamespace.Length + 1);
                }
                if (name.StartsWith($"{ServiceNamespace}."))
                {
                    name = name.Substring(ServiceNamespace.Length + 1);
                }
                else if (name.StartsWith($"{TypeNamespace}."))
                {
                    name = name.Substring(TypeNamespace.Length + 1);
                }

                return name;
            };
        }

        /// <summary>
        /// The namespace that the project belongs to. This namespace 
        /// will not be included in the type references.
        /// </summary>
        public string ProjectNamespace { get; set; }

        /// <summary>
        /// The namespace that will be added to the project namespace
        /// before adding the type or service namespace.
        /// </summary>
        public string CodeNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the types
        /// </summary>
        public string TypeNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the services(interfaces)
        /// </summary>
        public string ServiceNamespace { get; set; }

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
        /// The function that maps a type definition on to a reference name.
        /// </summary>
        public Func<ICSharpType, string> TypeDefinitionNameMapper { get; set; }

        /// <summary>
        /// The function that maps the method name to a path.
        /// </summary>
        public Func<string, string> MapPath { get; set; }

    }
}