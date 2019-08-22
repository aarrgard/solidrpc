using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcAbstractionProvider(typeof(ISolidRpcContentHandler), typeof(SolidRpcContentHandler))]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Implements logic to get content
    /// </summary>
    public class SolidRpcContentHandler : ISolidRpcContentHandler
    {
        /// <summary>
        /// The content handler
        /// </summary>
        /// <param name="contentStore"></param>
        /// <param name="methodBinderStore"></param>
        public SolidRpcContentHandler(SolidRpcContentStore contentStore, IMethodBinderStore methodBinderStore)
        {
            ContentStore = contentStore;
            MethodBinderStore = methodBinderStore;
            StaticFiles = new ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>>();
        }
        private SolidRpcContentStore ContentStore { get; }
        private IMethodBinderStore MethodBinderStore { get; }

        /// <summary>
        /// The static files configured for this host
        /// </summary>
        private ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>> StaticFiles { get; }

        /// <summary>
        /// The path prefixes
        /// </summary>
        public IEnumerable<string> PathPrefixes
        {
            get
            {
                return ContentStore.StaticContents.SelectMany(o => GetPathPrefixes(o)).Distinct();
            }
        }


        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FileContent> GetContent(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
            {
                throw new FileContentNotFoundException();
            }
            return StaticFiles.GetOrAdd(path, GetStaticContentInternal).Invoke(path, cancellationToken);
        }

        private IEnumerable<string> GetPathPrefixes(SolidRpcContentStore.StaticContent c)
        {

            if(!string.IsNullOrEmpty(c.PathPrefix))
            {
                return new[] { c.PathPrefix };
            }
            else
            {
                return MethodBinderStore.MethodBinders
                        .Where(mb => mb.Assembly == c.Assembly)
                        .Select(o => o.OpenApiSpec.BaseAddress.AbsolutePath);                    
            }
        }

        private Func<string, CancellationToken, Task<FileContent>> GetStaticContentInternal(string path)
        {
            var pathMappings = ContentStore.StaticContents.SelectMany(o => GetPathPrefixes(o).Select(o2 => new
            {
                PathPrefix = o2,
                Content = o
            })).ToList();

            var staticFiles = pathMappings
                .Where(o => path.StartsWith(o.PathPrefix))
                .Where(o => {
                    return path.Substring(o.PathPrefix.Length).Replace('/', '.').Equals(o.Content.PathName);
                }).Select(o => o.Content).ToList();

            if (!staticFiles.Any())
            {
                return (_, cancellationToken) => throw new FileContentNotFoundException();
            }
            var staticFile = staticFiles.First();

            var assemblyLocation = new FileInfo(staticFile.Assembly.Location);
            assemblyLocation = assemblyLocation.Exists ? assemblyLocation : null;

            return (_, cancellationToken) =>
            {
                return Task.FromResult(new FileContent()
                {
                    ContentType = staticFile.ContentType,
                    Content = staticFile.Assembly.GetManifestResourceStream(staticFile.ResourceName),
                    LastModified = assemblyLocation?.LastWriteTime
                });
            };
        }
    }
}
