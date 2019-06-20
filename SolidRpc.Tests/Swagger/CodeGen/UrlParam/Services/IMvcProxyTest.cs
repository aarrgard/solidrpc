using System.Threading.Tasks;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.UrlParam.Services {
    /// <summary>
    /// 
    /// </summary>
    public interface IMvcProxyTest {
        /// <summary>
        /// Sends a boolean back and forth between client and server
        /// </summary>
        /// <param name="p">The number to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<bool> ProxyIntegerInPath(
            long p,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}