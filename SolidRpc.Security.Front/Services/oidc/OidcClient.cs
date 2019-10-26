using SolidRpc.Security.Front.InternalServices;
using SolidRpc.Security.Services.Oidc;
using SolidRpc.Security.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Front.Services.oidc
{
    /// <summary>
    /// Implements logic to fetch the configuration for an oidc client.
    /// </summary>
    public class OidcClient : IOidcClient
    {
        public OidcClient(ISolidRpcSecurityOptions solidRpcSecurityOptions)
        {
            SolidRpcSecurityOptions = solidRpcSecurityOptions;
        }

        private ISolidRpcSecurityOptions SolidRpcSecurityOptions { get; }

        /// <summary>
        /// Returns the settings.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Settings> Settings(CancellationToken cancellationToken = default(CancellationToken))
        {
            var settings = new Settings();
            settings.Authority = SolidRpcSecurityOptions.Authority;
            settings.ClientId = SolidRpcSecurityOptions.ClientId;
            settings.ResponseType = "id_token";
            settings.Scope = "openid";
            return Task.FromResult(settings);
        }
    }
}
