using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using SolidRpc.OpenApi.AzFunctions.Types;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// Contains the static content
    /// </summary>
    public class SolidRpcStaticContent : ISolidRpcStaticContent
    {
        private class StaticContent
        {
            public string AbsolutePath { get; set; }
            public string PathName { get; set; }
            public Assembly Assembly { get; set; }
            public string ResourceName { get; set; }
            public string ContentType { get; set; }
        }
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcStaticContent()
        {
            StaticContents = new List<StaticContent>();
            StaticFiles = new ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>>();
            ContentTypeProvider = new FileExtensionContentTypeProvider();
        }

        /// <summary>
        /// The content type provider
        /// </summary>
        public IContentTypeProvider ContentTypeProvider { get; }

        /// <summary>
        /// The static files configured for this host
        /// </summary>
        private ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>> StaticFiles { get; }

        private IList<StaticContent> StaticContents { get; }

        /// <summary>
        /// Adds some content
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="packagePath"></param>
        /// <param name="absolutePath"></param>
        public void AddContent(Assembly assembly, string packagePath, string absolutePath)
        {
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
                    AbsolutePath = absolutePath,
                    ContentType = contentType
                });
            }
        }

        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FileContent> GetStaticContent(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(path == null)
            {
                throw new FileContentNotFoundException();
            }
            return StaticFiles.GetOrAdd(path, GetStaticContentInternal).Invoke(path, cancellationToken);
        }

        private Func<string, CancellationToken, Task<FileContent>> GetStaticContentInternal(string path)
        {
            var staticFiles = StaticContents
                .Where(o => path.StartsWith(o.AbsolutePath))
                .Where(o => {
                    return path.Substring(o.AbsolutePath.Length).Replace('/', '.').Equals(o.PathName);
                }).ToList();

            if(staticFiles.Count != 1)
            {
                return (_, cancellationToken) => throw new FileContentNotFoundException();
            }
            var staticFile = staticFiles.First();
            return (_, cancellationToken) =>
            {
                return Task.FromResult(new FileContent()
                {
                    ContentType = staticFile.ContentType,
                    Content = staticFile.Assembly.GetManifestResourceStream(staticFile.ResourceName)
                });
            };
        }
    }
}
