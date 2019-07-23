using SolidRpc.OpenApi.Binder.Proxy;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Interface that is used to setup the solid rpc infrastructure
    /// </summary>
    public interface ISolidRpcSetup
    {
        /// <summary>
        /// Performes the initialization work
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Setup(CancellationToken cancellationToken = default(CancellationToken));
    }
}