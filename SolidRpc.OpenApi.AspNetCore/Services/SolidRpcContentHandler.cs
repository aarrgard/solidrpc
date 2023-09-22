using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.Binder.Http;
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
        /// <param name="serviceProvider"></param>
        /// <param name="methodAddressTransformer"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="contentStore"></param>
        public SolidRpcContentHandler(
            IServiceProvider serviceProvider,
            IMethodAddressTransformer methodAddressTransformer,
            IMethodBinderStore methodBinderStore,
            SolidRpcContentStore contentStore)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            MethodAddressTransformer = methodAddressTransformer ?? throw new ArgumentNullException(nameof(methodAddressTransformer));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            ContentStore = contentStore ?? throw new ArgumentNullException(nameof(contentStore));
            StaticFiles = new ConcurrentDictionary<string, Func<string, CancellationToken, Task<FileContent>>>();
        }

        private IServiceProvider ServiceProvider { get; }
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
                return ContentStore.StaticContents.SelectMany(o => GetPathPrefixes(o))
                    .Union(ContentStore.Rewrites.Select(o => o[0]))
                    .Distinct();
            }
        }

        /// <summary>
        /// Returns the path mappings that we add to the proxy
        /// </summary>
        public async Task<IEnumerable<NameValuePair>> GetPathMappingsAsync(bool redirects, CancellationToken cancellationToken)
        {
            using (var scope = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
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
        public async Task<FileContent> GetContent(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
            {
                throw new FileContentNotFoundException();
            }

            //
            // check path mappings
            //
            if (ContentStore.DynamicContents.TryGetValue(path, out DynamicMapping dm))
            {
                using (var ss = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var uri = (await dm.UriResolver(ss.ServiceProvider));
                    var sUri = uri.ToString();

                    // determine if this is a local path
                    var localPaths = await ss.ServiceProvider.GetRequiredService<ISolidRpcHost>().AllowedCorsOrigins(cancellationToken);
                    var isLocal = localPaths.Any(o => sUri.StartsWith(o));
                    if (isLocal)
                    {
                        var invoc = ss.ServiceProvider.GetRequiredService<IMethodInvoker>();
                        
                        var httpRequest = new SolidHttpRequest();
                        httpRequest.Method = "GET";
                        httpRequest.HostAndPort = $"{uri.Host}:{uri.Port}";
                        httpRequest.Path = uri.AbsolutePath;

                        var transport = ss.ServiceProvider.GetRequiredService<IEnumerable<ITransportHandler>>().Single(o => o.TransportType == "Http");
                        var resp = await invoc.InvokeAsync(ss.ServiceProvider, transport, httpRequest, cancellationToken);

                        var okStatusCode = new[] { 200, 204, 302 };
                        if(!okStatusCode.Contains(resp.StatusCode))
                        {
                            throw new FileContentNotFoundException();
                        }

                        return new FileContent()
                        {
                            Content = resp.ResponseStream,
                            CharSet = resp.CharSet,
                            ContentType = resp.MediaType,
                            Location = resp.Location,
                            ETag = resp.ETag,
                            LastModified = resp.LastModified
                        };
                    }
                    else
                    {
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
                }
            }

            return await StaticFiles.GetOrAdd(path, GetStaticContentInternal).Invoke(path, cancellationToken);
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
                        .Select(o => $"{o.HostedAddress.AbsolutePath}*")
                        .Select(o => MethodAddressTransformer.RewritePath(o));                    
            }
        }

        private Func<string, CancellationToken, Task<FileContent>> GetStaticContentInternal(string path)
        {
            //
            // check static content
            //
            var pathMappings = ContentStore.StaticContents.SelectMany(o => GetPathPrefixes(o).Select(o2 => new
            {
                PathPrefix = o2.EndsWith("*") ? o2.Substring(0,o2.Length-1) : o2,
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
                if (string.IsNullOrEmpty(ContentStore.NotFoundRewrite) || path == ContentStore.NotFoundRewrite)
                {
                    return (_, cancellationToken) =>
                    {
                        throw new FileContentNotFoundException($"Cannot locate resource {_}");
                    };
                }
                else
                {
                    return (_, CancellationToken) =>
                    {
                        return GetContent(ContentStore.NotFoundRewrite);
                    };
                }
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
        public Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            return GetProtectedContentAsync(resource, null, cancellationToken);
        }

        /// <summary>
        /// Returns a protected content.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FileContent> GetProtectedContentAsync(byte[] resource, string fileName, CancellationToken cancellationToken = default)
        {
            using (var scope = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var protectedResource = scope.ServiceProvider.GetService<ISolidRpcProtectedResource>();
                if (protectedResource == null) throw new FileContentNotFoundException("Protected resource handler not registered");
                return await protectedResource.GetProtectedContentAsync(resource, fileName, cancellationToken);
            }
        }
    }
}
