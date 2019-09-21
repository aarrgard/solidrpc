using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Facebook;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.Facebook
{
    public class FacebookLocal : LoginProviderBase, IFacebookLocal
    {
        public FacebookLocal(FacebookOptions facebookOptions, IMethodBinderStore methodBinderStore)
        {
            FacebookOptions = facebookOptions;
            MethodBinderStore = methodBinderStore;
        }

        public FacebookOptions FacebookOptions { get; }

        public IMethodBinderStore MethodBinderStore { get; }

        public override string ProviderName => "Facebook";

        public string ButtonHtml => $"<fb:login-button scope=\"{FacebookOptions.RequestedScopes}\" onlogin=\"checkLoginState();\"></fb:login-button>";

        public override async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LoginProvider() {
                Name = ProviderName,
                Meta = new LoginProviderMeta[0],
                Script = new[] { await MethodBinderStore.GetUrlAsync<IFacebookLocal>(o => o.LoginScript(cancellationToken)) },
                Status = "NotLoggedIn",
                ButtonHtml = ButtonHtml
            };
        }

        public Task<string> LoggedIn(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(accessToken);
        }

        public Task<string> LoggedOut(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loggedInPostback = (await MethodBinderStore.GetUrlAsync<IFacebookLocal>(o => o.LoggedIn("{accessToken}", cancellationToken))).ToString();
            var logoutPostback = (await MethodBinderStore.GetUrlAsync<IFacebookLocal>(o => o.LoggedOut("{accessToken}", cancellationToken))).ToString();
            return await GetManifestResourceAsWebContent("FacebookLocal.LoginScript.js", new Dictionary<string, string>()
            {
                { "{your-app-id}", FacebookOptions.AppId },
                { "{api-version}", "v4.0" },
                { "{loggedin-postback}", loggedInPostback },
                { "{logout-postback}", logoutPostback }
            });
        }
    }
}
