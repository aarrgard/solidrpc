using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// Interface that is used to setup the solid rpc infrastructure
    /// </summary>
    public interface ISolidRpcHost
    {
        /// <summary>
        /// Performes the initialization work
        /// </summary>
        /// <param name="configurationHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CheckConfig(string configurationHash = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}