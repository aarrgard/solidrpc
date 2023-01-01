using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model.CSharp;
using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.Generator
{
    /// <summary>
    /// Base class for generating a OpenApi specification.
    /// </summary>
    public abstract class OpenApiSpecGenerator
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="settings"></param>
        public OpenApiSpecGenerator(SettingsSpecGen settings = null)
        {
            Settings = settings;
            TypeDefinitionNameMapper = c => {
                var name = c.FullName;
                if (name.StartsWith($"{Settings.ProjectNamespace}."))
                {
                    name = name.Substring(Settings.ProjectNamespace.Length + 1);
                }
                if (name.StartsWith($"{Settings.CodeNamespace}."))
                {
                    name = name.Substring(Settings.CodeNamespace.Length + 1);
                }
                if (name.StartsWith($"{Settings.ServiceNamespace}."))
                {
                    name = name.Substring(Settings.ServiceNamespace.Length + 1);
                }
                else if (name.StartsWith($"{Settings.TypeNamespace}."))
                {
                    name = name.Substring(Settings.TypeNamespace.Length + 1);
                }

                name = name.Replace('+', '.'); // fix inner class name problem. no + allowed
                return name;
            };
            MapPath = s => $"/{s.Replace('.', '/').Replace('+','/')}";
        }

        /// <summary>
        /// The settings for spec generation.
        /// </summary>
        public SettingsSpecGen Settings { get; }

        /// <summary>
        /// The function that maps a type definition on to a reference name.
        /// </summary>
        public Func<ICSharpType, string> TypeDefinitionNameMapper { get; set; }

        /// <summary>
        /// The function that maps the method name to a path.
        /// </summary>
        public Func<string, string> MapPath { get; set; }

        /// <summary>
        /// Constructs an openapi spec from the supplied cSharpRepository.
        /// </summary>
        /// <param name="openApiSpecResolver"></param>
        /// <param name="cSharpRepository"></param>
        /// <returns></returns>
        public abstract IOpenApiSpec CreateSwaggerSpec(IOpenApiSpecResolver openApiSpecResolver, ICSharpRepository cSharpRepository);
    }
}
