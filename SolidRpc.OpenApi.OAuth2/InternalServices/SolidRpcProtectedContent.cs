using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.OAuth2.InternalServices;
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
        public SolidRpcProtectedContent(IAuthorityLocal authority = null)
        {
            Authority = authority;
        }

        private IAuthorityLocal Authority { get; }

        public Task<IEnumerable<byte[]>> CreateProtectedResourceStrings(IEnumerable<string> content, CancellationToken cancellationToken)
        {
            var key = Authority.PublicSigningKey;
            return Task.FromResult(content.Select(o => Authority.Encrypt(Encoding.UTF8.GetBytes(o))));
        }

        public Task<FileContent> GetProtectedContentAsync(byte[] resource, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<FileContent> GetProtectedContentAsync(string resource, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
