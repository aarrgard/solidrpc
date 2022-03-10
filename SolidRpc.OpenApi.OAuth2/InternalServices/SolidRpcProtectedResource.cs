using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
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
            ISolidRpcProtectedContent solidRpcProtectedContent = null,
            IAuthorityLocal authority = null)
        {
            Logger = logger;
            Authority = authority;
            SolidRpcProtectedContent = solidRpcProtectedContent;
        }

        private ILogger Logger { get; }
        private IAuthorityLocal Authority { get; }
        private ISolidRpcProtectedContent SolidRpcProtectedContent { get; }

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


        public async Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            var pr = await UnprotectAsync(resource, cancellationToken);
            if(pr.Expiration < DateTimeOffset.UtcNow)
            {
                throw new FileContentNotFoundException("Resource expired");
            }

            if(pr.Source == Authority.Authority)
            {
                return await SolidRpcProtectedContent.GetProtectedContentAsync(pr.Resource, cancellationToken);
            }
            else
            {
                throw new FileContentNotFoundException("Content not local");
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
