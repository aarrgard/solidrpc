using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Generator.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.Generator.Impl
{
    /// <summary>
    /// Resolves specifications that are embedded in an assembly.
    /// </summary>
    public class OpenApiSpecResolverProject : IOpenApiSpecResolver
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OpenApiSpecResolverProject(IOpenApiParser openApiParser, Project project)
        {
            OpenApiParser = openApiParser ?? throw new System.ArgumentNullException(nameof(openApiParser));
            OpenApiSpecs = new ConcurrentDictionary<string, IOpenApiSpec>();
            Project = project;
        }

        private ConcurrentDictionary<string, IOpenApiSpec> OpenApiSpecs { get; }
        private Project Project { get; }
        private IOpenApiParser OpenApiParser { get; }

        /// <summary>
        /// Resolves the api spec with supplied name
        /// </summary>
        /// <param name="address"></param>
        /// <param name="openApiSpec"></param>
        /// <returns></returns>
        public bool TryResolveApiSpec(string address, out IOpenApiSpec openApiSpec, string basePath = null)
        {
            if(basePath != null)
            {
                address = CreatePath(address.Split('/'), basePath.Split('/').Reverse().Skip(1).Reverse());
            }
            openApiSpec = OpenApiSpecs.GetOrAdd(address, _ => {
                var dirName = string.Join("/", _.Split('/').Reverse().Skip(1).Reverse());
                var fileName = _.Split('/').Reverse().Take(1).First();
                var projectFile = Project.ProjectFiles
                    .Where(o => o.Directory == dirName)
                    .Where(o => o.FileData.Filename == fileName)
                    .FirstOrDefault();
                if(projectFile == null)
                {
                    return null;
                }
                using (var sr = new StreamReader(projectFile.FileData.FileStream))
                {
                    return OpenApiParser.ParseSpec(this, address, sr.ReadToEnd());
                }
            });
            return openApiSpec != null;
        }

        private string CreatePath(IEnumerable<string> address, IEnumerable<string> basePath)
        {
            if (address.First() == ".")
            {
                return CreatePath(address.Skip(1), basePath);
            }
            if (address.First() == "..")
            {
                return CreatePath(address.Skip(1), basePath.Reverse().Skip(1).Reverse());
            }
            return string.Join("/", basePath.Union(address));
        }
    }
}
