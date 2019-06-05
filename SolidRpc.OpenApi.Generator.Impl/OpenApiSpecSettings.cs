using System;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Types;

namespace SolidRpc.OpenApi.Generator
{
    /// <summary>
    /// Settings for generating a swagger spec.
    /// </summary>
    public class OpenApiSpecSettings : SettingsSpecGen
    {
        /// <summary>
        /// Constructs a new settings instance
        /// </summary>
        public OpenApiSpecSettings()
        {
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
        /// The function that maps a type definition on to a reference name.
        /// </summary>
        public Func<ICSharpType, string> TypeDefinitionNameMapper { get; set; }

        /// <summary>
        /// The function that maps the method name to a path.
        /// </summary>
        public Func<string, string> MapPath { get; set; }

    }
}