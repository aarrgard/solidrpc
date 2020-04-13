using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
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
        public OidcClient(ISolidRpcSecurityOptions solidRpcSecurityOptions, IInvoker<IOidcClient> invoker)
        {
            SolidRpcSecurityOptions = solidRpcSecurityOptions;
            Invoker = invoker;
        }

        private ISolidRpcSecurityOptions SolidRpcSecurityOptions { get; }
        private IInvoker<IOidcClient> Invoker { get; }

        public Task LoggedIn(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns the settings.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Settings> Settings(CancellationToken cancellationToken = default(CancellationToken))
        {
            var redirectUri = await Invoker.GetUriAsync(o => o.LoggedIn(CancellationToken.None));
            var settings = new Settings();
            settings.Authority = SolidRpcSecurityOptions.Authority;
            settings.ClientId = SolidRpcSecurityOptions.ClientId;
            settings.ResponseType = "token";
            settings.Scope = "openid";
            settings.PopupRedirectUri = redirectUri.ToString();
            return settings;
        }
    }
}
