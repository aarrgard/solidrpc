using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.StaticFiles;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;

[assembly: SolidRpcAbstractionProvider(typeof(ISolidRpcContentStore), typeof(SolidRpcContentStore))]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Contains the static content
    /// </summary>
    public class SolidRpcContentStore : ISolidRpcContentStore
    {
        /// <summary>
        /// Represents a static content.
        /// </summary>
        public class StaticContent
        {
            /// <summary>
            /// The path prefix. If not set(null) - the base paths of the assebly will be used.
            /// </summary>
            public string PathPrefix { get; set; }
            public string PathName { get; set; }
            public Assembly Assembly { get; set; }
            public string ResourceName { get; set; }
            public string ContentType { get; set; }
        }
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcContentStore()
        {
            StaticContents = new List<StaticContent>();
            ContentTypeProvider = new FileExtensionContentTypeProvider();
            Registrations = new HashSet<string>();
        }

        /// <summary>
        /// The content type provider
        /// </summary>
        public IContentTypeProvider ContentTypeProvider { get; }

        public IList<StaticContent> StaticContents { get; }

        /// <summary>
        /// Keeps track of registrations so that we do not add resources more than once.
        /// </summary>
        private HashSet<string> Registrations { get; }

        /// <summary>
        /// Adds some content
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="packagePath"></param>
        /// <param name="absolutePath"></param>
        public void AddContent(Assembly assembly, string packagePath, string absolutePath)
        {
            //
            // avoid duplicate registrations
            //
            var regKey = $"{assembly.GetName().FullName}:{packagePath}:{absolutePath}";
            if(Registrations.Contains(regKey))
            {
                return;
            }
            Registrations.Add(regKey);

            // get the name of the assemblt
            var assemblyName = assembly.GetName().Name;
            foreach(var resourceName in assembly.GetManifestResourceNames())
            {
                var pathName = resourceName;
                if(pathName.StartsWith(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    pathName = pathName.Substring(assemblyName.Length);
                }
                if(!pathName.StartsWith($".{packagePath}.", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                pathName = pathName.Substring(packagePath.Length + 2);
                string contentType;
                if (!ContentTypeProvider.TryGetContentType(pathName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                StaticContents.Add(new StaticContent()
                {
                    Assembly = assembly,
                    ResourceName = resourceName,
                    PathName = pathName.ToLower(),
                    PathPrefix = absolutePath,
                    ContentType = contentType
                });
            }
        }
    }
}
