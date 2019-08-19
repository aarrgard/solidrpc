using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Facebook;
using SolidRpc.Security.Types;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.Facebook
{
    public class FacebookLocal : IFacebookLocal, ILoginProvider
    {
        public FacebookLocal(FacebookOptions facebookOptions, IMethodBinderStore methodBinderStore)
        {
            FacebookOptions = facebookOptions;
            MethodBinderStore = methodBinderStore;
        }

        public FacebookOptions FacebookOptions { get; }

        public IMethodBinderStore MethodBinderStore { get; }

        public string ProviderName => "Facebook";

        public string ButtonHtml => $"<fb:login-button scope=\"{FacebookOptions.RequestedScopes}\" onlogin=\"checkLoginState();\"></fb:login-button>";

        public async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LoginProvider() {
                Name = ProviderName,
                Meta = new LoginProviderMeta[0],
                Script = new[] { await MethodBinderStore.GetUrlAsync<IFacebookLocal>(o => o.LoginScript(cancellationToken)) },
                Status = "NotLoggedIn",
                ButtonHtml = ButtonHtml
            };
        }

        public Task LoggedIn(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task LoggedOut(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith("FacebookLocal.LoginScript.js", StringComparison.InvariantCultureIgnoreCase));
            using (var res = GetType().Assembly.GetManifestResourceStream(resName))
            {
                using (var sr = new StreamReader(res))
                {
                    var script = sr.ReadToEnd();

                    script = script.Replace("{your-app-id}", FacebookOptions.AppId);
                    script = script.Replace("{api-version}", "v4.0");

                    var enc = Encoding.UTF8;
                    return Task.FromResult(new WebContent()
                    {
                        Content = new MemoryStream(enc.GetBytes(script)),
                        ContentType = "text/javascript",
                        CharSet = enc.HeaderName
                    });
                }
            }
        }
    }
}
