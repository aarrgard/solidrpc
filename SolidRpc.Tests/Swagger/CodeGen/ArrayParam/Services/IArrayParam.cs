using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.ArrayParam.Services {
    /// <summary>
    /// 
    /// </summary>
    public interface IArrayParam {
        /// <summary>
        /// Sends an array back and forth between client and server
        /// </summary>
        /// <param name="p">The array to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<int>> ProxyArrayInQuery(
            IEnumerable<int> p,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}