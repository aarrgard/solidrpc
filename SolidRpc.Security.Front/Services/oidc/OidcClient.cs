using SolidRpc.Abstractions.OpenApi.Binder;
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
        public OidcClient(ISolidRpcSecurityOptions solidRpcSecurityOptions, IMethodBinderStore methodBinderStore)
        {
            SolidRpcSecurityOptions = solidRpcSecurityOptions;
            MethodBinderStore = methodBinderStore;
        }

        private ISolidRpcSecurityOptions SolidRpcSecurityOptions { get; }
        private IMethodBinderStore MethodBinderStore { get; }

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
            var redirectUri = await MethodBinderStore.GetUrlAsync<IOidcClient>(o => o.LoggedIn(CancellationToken.None));
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
