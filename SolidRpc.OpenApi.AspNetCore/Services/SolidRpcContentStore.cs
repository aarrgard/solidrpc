using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.OpenApi.AspNetCore.Services;

[assembly: SolidRpcService(typeof(ISolidRpcContentStore), typeof(SolidRpcContentStore))]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Contains the static content
    /// </summary>
    public class SolidRpcContentStore : ISolidRpcContentStore
    {
        /// <summary>
        /// Represents a dynamc mapping
        /// </summary>
        public class DynamicMapping
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="uriResolver"></param>
            /// <param name="isRedirect"></param>
            public DynamicMapping(Func<IServiceProvider, Task<Uri>> uriResolver, bool isRedirect)
            {
                IsRedirect = isRedirect;
                UriResolver = uriResolver;
            }

            /// <summary>
            /// Is this a redirect or proxy call.
            /// </summary>
            public bool IsRedirect { get; }

            /// <summary>
            /// The resolver
            /// </summary>
            public Func<IServiceProvider, Task<Uri>> UriResolver { get; }
        }
        /// <summary>
        /// Represents a static content.
        /// </summary>
        public class StaticContent
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="assembly"></param>
            /// <param name="resourceName"></param>
            /// <param name="pathName"></param>
            /// <param name="absolutePath"></param>
            /// <param name="apiAssembly"></param>
            /// <param name="contentType"></param>
            public StaticContent(
                Assembly assembly, 
                string resourceName, 
                string pathName, 
                string absolutePath, 
                Assembly apiAssembly,
                string contentType)
            {
                Assembly = assembly;
                ResourceName = resourceName;
                PathName = pathName;
                PathPrefix = absolutePath;
                ApiAssembly = apiAssembly;
                ContentType = contentType;

            }
            /// <summary>
            /// The path prefix. 
            /// </summary>
            public string PathPrefix { get; }

            /// <summary>
            /// The api assembly.
            /// </summary>
            public Assembly ApiAssembly { get; }

            /// <summary>
            /// The path name
            /// </summary>
            public string PathName { get; }

            /// <summary>
            /// The assembly where the resource resides
            /// </summary>
            public Assembly Assembly { get;}
            
            /// <summary>
            /// The resource name
            /// </summary>
            public string ResourceName { get; }

            /// <summary>
            /// Thwe content type.
            /// </summary>
            public string ContentType { get; }
        }
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcContentStore()
        {
            StaticContents = new List<StaticContent>();
            DynamicContents = new Dictionary<string, DynamicMapping>();
            ContentTypeProvider = new FileExtensionContentTypeProvider();
            Registrations = new HashSet<string>();
        }

        /// <summary>
        /// The content type provider
        /// </summary>
        public IContentTypeProvider ContentTypeProvider { get; }

        /// <summary>
        /// The registered contents
        /// </summary>
        public IList<StaticContent> StaticContents { get; }

        /// <summary>
        /// The registered contents
        /// </summary>
        public IDictionary<string, DynamicMapping> DynamicContents { get; }

        /// <summary>
        /// Keeps track of registrations so that we do not add resources more than once.
        /// </summary>
        private HashSet<string> Registrations { get; }

        /// <summary>
        /// Adds some content
        /// </summary>
        /// <param name="contentAssembly"></param>
        /// <param name="assemblyRelativeName"></param>
        /// <param name="absolutePath"></param>
        public void AddContent(Assembly contentAssembly, string assemblyRelativeName, string absolutePath)
        {
            AddContentInternal(contentAssembly, assemblyRelativeName, null, absolutePath);
        }

        /// <summary>
        /// Adds a content
        /// </summary>
        /// <param name="contentAssembly"></param>
        /// <param name="assemblyRelativeName"></param>
        /// <param name="apiAssembly"></param>
        public void AddContent(Assembly contentAssembly, string assemblyRelativeName, Assembly apiAssembly)
        {
            AddContentInternal(contentAssembly, assemblyRelativeName, apiAssembly, null);
        }

        private void AddContentInternal(Assembly assembly, string assemblyRelativeName, Assembly apiAssembly, string absolutePath)
        {
            //
            // avoid duplicate registrations
            //
            var regKey = $"{assembly.GetName().Name}:{assemblyRelativeName}:{apiAssembly?.GetName()?.Name}:{absolutePath}";
            if (Registrations.Contains(regKey))
            {
                return;
            }
            Registrations.Add(regKey);

            // get the name of the assemblt
            var assemblyName = assembly.GetName().Name;
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                var pathName = resourceName;
                if (pathName.StartsWith(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    pathName = pathName.Substring(assemblyName.Length);
                }
                if (!pathName.StartsWith($".{assemblyRelativeName}.", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                pathName = pathName.Substring(assemblyRelativeName.Length + 2);
                string contentType;
                if (!ContentTypeProvider.TryGetContentType(pathName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                StaticContents.Add(new StaticContent(assembly, resourceName, pathName.ToLower(), absolutePath, apiAssembly, contentType));
            }
        }

        /// <summary>
        /// Adds a dynamic mapping between supplied path and the mapping function.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mapping"></param>
        /// <param name="isRedirect"></param>
        public void AddMapping(string path, Func<IServiceProvider, Task<Uri>> mapping, bool isRedirect)
        {
            DynamicContents[path] = new DynamicMapping(mapping, isRedirect);
        }
    }
}
