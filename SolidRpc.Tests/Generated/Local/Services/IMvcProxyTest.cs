using System.Threading.Tasks;
using System.Threading;
using System;
namespace SolidRpc.Tests.Generated.Local.Services {
    /// <summary>
    /// 
    /// </summary>
    public interface IMvcProxyTest {
        /// <summary>
        /// Sends a boolean back and forth between client and server
        /// </summary>
        /// <param name="b">The boolean to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<bool> ProxyBoolean(
            bool b,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <param name="s">The short to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyShort(
            int s,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyInt(
            int i,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an long back and forth between client and server
        /// </summary>
        /// <param name="l">The long to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<long> ProxyLong(
            long l,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<string> ProxyString(
            string s,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a guid back and forth between client and server
        /// </summary>
        /// <param name="g">The guid to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<Guid> ProxyGuid(
            Guid g,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}