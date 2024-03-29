using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Security.Types;
using System.Threading;
namespace SolidRpc.Security.Services {
    /// <summary>
    /// Defines logic for solid rpc security clients
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ISolidRpcSecurityClient {
        /// <summary>
        /// Returns all the registered clients
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Client>> GetClient(
            string clientId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}