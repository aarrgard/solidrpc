using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.UrlParam.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IUrlParam {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="p">The number to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyIntegerInPath(
            int p,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="arr">The array to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<int>> ProxyArrayInPathCsv(
            IEnumerable<int> arr,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="arr">The array to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<int>> ProxyArrayInPathPipe(
            IEnumerable<int> arr,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}