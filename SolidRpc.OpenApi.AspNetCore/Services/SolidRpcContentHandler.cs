using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static SolidRpc.OpenApi.AspNetCore.Services.SolidRpcContentStore;

[assembly: SolidRpcService(typeof(ISolidRpcContentHandler), typeof(SolidRpcContentHandler))]
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
        /// <param name="serviceScopeFactory"></param>
        /// <param name="protectedContent"></param>
        /// <param name="methodAddressTransformer"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="contentStore"></param>
        public SolidRpcContentHandler(
            IServiceScopeFactory serviceScopeFactory,
            IMethodAddressTransformer methodAddressTransformer,
            IMethodBinderStore methodBinderStore,
            SolidRpcContentStore contentStore,
            ISolidRpcProtectedContent protectedContent = null)
        {
            ServiceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            MethodAddressTransformer = methodAddressTransformer ?? throw new ArgumentNullException(nameof(methodAddressTransformer));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            ContentStore = contentStore ?? throw new ArgumentNullException(nameof(contentStore));
            ProtectedContent = protectedContent;
            StaticFiles = new ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>>();
        }

        private IServiceScopeFactory ServiceScopeFactory { get; }
        private ISolidRpcProtectedContent ProtectedContent { get; }
        private IMethodAddressTransformer MethodAddressTransformer { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private SolidRpcContentStore ContentStore { get; }

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
        /// Returns the path mappings that we add to the proxy
        /// </summary>
        public async Task<IEnumerable<NameValuePair>> GetPathMappingsAsync(bool redirects, CancellationToken cancellationToken)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var tasks = ContentStore.DynamicContents
                    .Where(o => o.Value.IsRedirect == redirects)
                    .Select(async o =>
                    {
                        return new NameValuePair()
                        {
                            Name = o.Key,
                            Value = (await o.Value.UriResolver(scope.ServiceProvider))?.ToString()
                        };
                    });
                return (await Task.WhenAll(tasks));
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
            return StaticFiles.GetOrAdd(path, GetContentInternal).Invoke(path, cancellationToken);
        }

        private IEnumerable<string> GetPathPrefixes(StaticContent c)
        {
            if (!string.IsNullOrEmpty(c.PathPrefix))
            {
                return new[] { MethodAddressTransformer.RewritePath(c.PathPrefix) };
            }
            else
            {
                return MethodBinderStore.MethodBinders
                        .Where(mb => mb.Assembly == c.ApiAssembly)
                        .Select(o => o.HostedAddress.AbsolutePath)
                        .Select(o => MethodAddressTransformer.RewritePath(o));                    
            }
        }

        private Func<string, CancellationToken, Task<FileContent>> GetContentInternal(string path)
        {
            //
            // check path mappings
            //
            if(ContentStore.DynamicContents.TryGetValue(path, out DynamicMapping dm))
            {
                return async (_, cancellationToken) =>
                {
                    using(var ss = ServiceScopeFactory.CreateScope())
                    {
                        var uri = await dm.UriResolver(ss.ServiceProvider);
                        var httpClient = ss.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
                        var resp = await httpClient.GetAsync(uri, cancellationToken);
                        var ms = new MemoryStream();
                        await resp.Content.CopyToAsync(ms);

                        return new FileContent()
                        {
                            Content = new MemoryStream(ms.ToArray()),
                            CharSet = resp.Content?.Headers?.ContentType?.CharSet,
                            ContentType = resp.Content?.Headers?.ContentType?.MediaType,
                            Location = resp.Headers?.Location?.ToString(),
                            ETag = resp.Headers?.ETag?.Tag,
                            LastModified = resp.Content?.Headers?.LastModified
                        };
                    }
                 };
            }

            //
            // check static content
            //
            var pathMappings = ContentStore.StaticContents.SelectMany(o => GetPathPrefixes(o).Select(o2 => new
            {
                PathPrefix = o2,
                Content = o
            })).ToList();

            var staticFiles = pathMappings
                .Where(o => path.StartsWith(o.PathPrefix))
                .Where(o => {
                    var contentPath = path.Substring(o.PathPrefix.Length);
                    var resourceName = contentPath.Replace('/', '.');
                    return resourceName.Equals(o.Content.PathName, StringComparison.InvariantCultureIgnoreCase);
                }).Select(o => o.Content).ToList();

            if (!staticFiles.Any()) 
            {
                return (_, cancellationToken) =>
                {
                    throw new FileContentNotFoundException($"Cannot locate resource {_}");
                };
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

        /// <summary>
        /// Returns the protected content
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            return ProtectedContent.GetProtectedContentAsync(resource, cancellationToken);
        }
    }
}
