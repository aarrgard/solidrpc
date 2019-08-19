using SolidRpc.Security.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services
{
    /// <summary>
    /// Interface implemented by all the login providers.
    /// </summary>
    public interface ILoginProvider
    {
        /// <summary>
        /// The name of the provider
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Returns information about the login provider
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken));
    }
}
