using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcProtectedResource), typeof(SolidRpcProtectedResource), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcProtectedResource : ISolidRpcProtectedResource
    {
        public SolidRpcProtectedResource(
            ILogger<SolidRpcProtectedResource> logger,
            IInvoker<ISolidRpcProtectedContent> contentInvoker,
            IAuthorityLocal authority = null)
        {
            Logger = logger;
            Authority = authority;
            ContentInvoker = contentInvoker;
        }

        private ILogger Logger { get; }
        private IAuthorityLocal Authority { get; }
        private IInvoker<ISolidRpcProtectedContent> ContentInvoker { get; }

        public Task<byte[]> ProtectAsync(string content, DateTimeOffset expiryTime, CancellationToken cancellationToken)
        {
            var auth = Authority.Authority;
            return (new ProtectedResource()
                {
                    Resource = content,
                    Source = Authority.Authority,
                    Expiration = expiryTime
                }).ToByteArrayAsync((source, barr) => Authority.SignHash(barr, cancellationToken));
        }


        public async Task<FileContent> GetProtectedContentAsync(ProtectedResource resource, CancellationToken cancellationToken)
        {
            if(resource.Expiration < DateTimeOffset.UtcNow)
            {
                throw new FileContentNotFoundException("Resource expired");
            }

            if(resource.Source == Authority.Authority)
            {
                return await ContentInvoker.InvokeAsync(o => o.GetProtectedContentAsync(resource.Resource, cancellationToken), opts => {
                    return opts.SetTransport("Local");
                });
            }
            else
            {
                return await ContentInvoker.InvokeAsync(o => o.GetProtectedContentAsync(resource.Resource, cancellationToken), opts => {
                    return opts.SetTransport("Http").AddPreInvokeCallback(r =>
                    {
                        var uri = new Uri(Authority.Authority);
                        r.HostAndPort = $"{uri.Host}:{uri.Port}";    
                        return Task.CompletedTask;
                    });
                });
            }
        }

        public Task<ProtectedResource> UnprotectAsync(byte[] resource, CancellationToken cancellationToken)
        {
            return resource.AsProtectedResourceAsync(async (source, data, signature) =>
            {
                var prAuth = Authority.AuthorityFactory.GetAuthority(source);
                return await prAuth.VerifyData(data, signature, cancellationToken);
            });
        }
    }
}
