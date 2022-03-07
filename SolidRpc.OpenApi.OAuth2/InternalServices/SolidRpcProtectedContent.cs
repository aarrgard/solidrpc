using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcProtectedContent), typeof(SolidRpcProtectedContent), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcProtectedContent : ISolidRpcProtectedContent
    {
        public SolidRpcProtectedContent(
            ILogger<SolidRpcProtectedContent> logger,
            IAuthorityLocal authority = null)
        {
            Logger = logger;
            Authority = authority;
        }

        private ILogger Logger { get; }
        private IAuthorityLocal Authority { get; }

        public Task<IEnumerable<byte[]>> CreateProtectedResourceStringsAsync(IEnumerable<string> content, CancellationToken cancellationToken)
        {
            return Task.FromResult(content.Select(o => Authority.Encrypt(Encoding.UTF8.GetBytes(o))));
        }

        public Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            byte[] decResource;
            try
            {
                decResource = Authority.Decrypt(resource);
            } 
            catch(Exception e)
            {
                Logger.LogError(e, "Failed to decrypt resource identifier");
                throw new FileContentNotFoundException();
            }
            return GetProtectedContentAsync(Encoding.UTF8.GetString(decResource), cancellationToken);
        }

        public virtual Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken)
        {
            Logger.LogError("No protected content found:"+resource);
            throw new FileContentNotFoundException();
        }
    }
}
