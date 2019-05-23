using System.Threading.Tasks;
using System.Threading;
namespace SolidRpc.Tests.Generated.Local.Services {
    /// <summary>
    /// 
    /// </summary>
    public interface IMvcProxyTest {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The integer to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyInt(
            int i,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<string> ProxyString(
            string s,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}