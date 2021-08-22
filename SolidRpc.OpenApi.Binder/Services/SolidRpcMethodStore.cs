using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcMethodStore), typeof(SolidRpcMethodStore), SolidRpcServiceLifetime.Transient)]
namespace SolidRpc.OpenApi.Binder.Services
{
    public class SolidRpcMethodStore : ISolidRpcMethodStore
    {
        public SolidRpcMethodStore(ISolidRpcAuthorization authorization)
        {
            Authorization = authorization;
        }

        public ISolidRpcAuthorization Authorization { get; }

        public Task<IEnumerable<HttpRequest>> GetHttpRequestAsync(int? takeCount = 1, CancellationToken cancellation = default(CancellationToken))
        {
            return GetHttpRequestsAsync(Authorization?.SessionId, takeCount, cancellation);
        }

        public Task<IEnumerable<HttpRequest>> GetHttpRequestsAsync(string sessionId, int? takeCount = 1, CancellationToken cancellation = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveHttpRequestAsync(string solidRpcCallId, CancellationToken cancellation = default(CancellationToken))
        {
            return RemoveHttpRequestAsync(Authorization?.SessionId, solidRpcCallId, cancellation);
        }

        public Task RemoveHttpRequestAsync(string sessionId, string solidRpcCallId, CancellationToken cancellation = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}
