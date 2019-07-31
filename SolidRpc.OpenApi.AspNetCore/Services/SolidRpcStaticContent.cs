﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;

[assembly: SolidRpcAbstractionProvider(typeof(ISolidRpcStaticContent), typeof(SolidRpcStaticContent))]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// Contains the static content
    /// </summary>
    public class SolidRpcStaticContent : ISolidRpcStaticContent
    {
        private class StaticContent
        {
            public string PathPrefix { get; set; }
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
        /// Returns all the paths
        /// </summary>
        public IEnumerable<string> PathPrefixes => StaticContents.Select(o => o.PathPrefix).Distinct();

        /// <summary>
        /// Adds some content
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="packagePath"></param>
        /// <param name="absolutePath"></param>
        public void AddContent(Assembly assembly, string packagePath, string absolutePath)
        {
            if(!absolutePath.EndsWith("/"))
            {
                absolutePath = absolutePath + "/";
            }
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
                .Where(o => path.StartsWith(o.PathPrefix))
                .Where(o => {
                    return path.Substring(o.PathPrefix.Length).Replace('/', '.').Equals(o.PathName);
                }).ToList();

            if(!staticFiles.Any())
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
