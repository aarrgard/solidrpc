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

[assembly: SolidRpcService(typeof(ISolidRpcProtectedResource), typeof(SolidRpcProtectedResource), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcProtectedResource : ISolidRpcProtectedResource
    {
        private static readonly Guid s_RecurseProtection = Guid.NewGuid();

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="contentHandlerInvoker"></param>
        /// <param name="protectedContent"></param>
        /// <param name="authorityFactory"></param>
        /// <param name="authority"></param>
        public SolidRpcProtectedResource(
            IInvoker<ISolidRpcContentHandler> contentHandlerInvoker,
            ISolidRpcProtectedContent protectedContent = null,
            IAuthorityFactory authorityFactory = null,
            IAuthorityLocal authority = null)
        {
            Authority = authority;
            AuthorityFactory = authorityFactory;
            ProtectedContent = protectedContent;
            ContentHandlerInvoker = contentHandlerInvoker;
        }
        private IAuthorityLocal Authority { get; }
        private IAuthorityFactory AuthorityFactory { get; }
        private ISolidRpcProtectedContent ProtectedContent { get; }
        private IInvoker<ISolidRpcContentHandler> ContentHandlerInvoker { get; }

        public Task<byte[]> ProtectAsync(string content, DateTimeOffset expiryTime, CancellationToken cancellationToken)
        {
            return ProtectAsync(new ProtectedResource()
            {
                Resource = content,
                Source = Authority.Authority,
                Expiration = expiryTime
            });
        }

        public Task<byte[]> ProtectAsync(ProtectedResource resource, CancellationToken cancellationToken = default)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            var auth = Authority.Authority;
            return resource.ToByteArrayAsync((source, barr) => Authority.SignHash(barr, cancellationToken));
        }

        public Task<ProtectedResource> UnprotectAsync(byte[] resource, CancellationToken cancellationToken)
        {
            if (AuthorityFactory == null) throw new Exception("No authority factory registered.");
            return resource.AsProtectedResourceAsync(async (source, data, signature) =>
            {
                var prAuth = AuthorityFactory.GetAuthority(source);
                return await prAuth.VerifyData(data, signature, cancellationToken);
            });
        }


        public async Task<FileContent> GetProtectedContentAsync(byte[] b64, string fileName, CancellationToken cancellationToken)
        {
            var resource = await UnprotectAsync(b64, cancellationToken);
            if(resource.Expiration < DateTimeOffset.UtcNow)
            {
                throw new FileContentNotFoundException("Resource expired");
            }

            if(resource.Source == Authority.Authority)
            {
                return await ProtectedContent.GetProtectedContentAsync(resource.Resource, cancellationToken);
            }
            else if(fileName == s_RecurseProtection.ToString())
            {
                return await ProtectedContent.GetProtectedContentAsync(resource.Resource, cancellationToken);
            }
            else
            {
                return await ContentHandlerInvoker.InvokeAsync(o => o.GetProtectedContentAsync(b64, s_RecurseProtection.ToString(), cancellationToken), opts => {
                    return opts.SetTransport("Http").AddPreInvokeCallback(r =>
                    {
                        var uri = new Uri(resource.Source);
                        r.HostAndPort = $"{uri.Host}:{uri.Port}";    
                        return Task.CompletedTask;
                    });
                });
            }
        }
    }
}
