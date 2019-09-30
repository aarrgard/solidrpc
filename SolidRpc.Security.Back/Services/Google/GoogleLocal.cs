using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Google;
using SolidRpc.Security.Types;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Back.Services.Google
{
    public class GoogleLocal : LoginProviderBase, IGoogleLocal
    {
        public GoogleLocal(GoogleOptions googleOptions, IMethodBinderStore methodBinderStore)
        {
            GoogleOptions = googleOptions;
            MethodBinderStore = methodBinderStore;
        }
        public override string ProviderName => "Google";

        public GoogleOptions GoogleOptions { get; }

        public IMethodBinderStore MethodBinderStore { get; }
        public string ButtonHtml => $"<div class=\"g-signin2\" data-onsuccess=\"onSignIn\" data-theme=\"dark\"></div>";

        public override async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LoginProvider()  
            {
                Name = ProviderName,
                Meta = new[] {
                    new LoginProviderMeta()
                    {
                        Name = "google-signin-scope",
                        Content = GoogleOptions.SigninScopes
                    },new LoginProviderMeta()
                    {
                        Name = "google-signin-client_id",
                        Content = GoogleOptions.ClientID
                    },
                },
                Script = new[] {
                    new Uri("https://apis.google.com/js/platform.js"),
                    await MethodBinderStore.GetUrlAsync<IGoogleLocal>(o => o.LoginScript(cancellationToken))
                },
                Status = "NotLoggedIn",
                ButtonHtml = ButtonHtml
            };
        }

        public Task<string> LoggedIn(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> LoggedOut(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetManifestResourceAsWebContent("GoogleLocal.LoginScript.js");
        }
    }
}
