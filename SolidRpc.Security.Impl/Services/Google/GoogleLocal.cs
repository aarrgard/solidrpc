using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Google;
using SolidRpc.Security.Types;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.Google
{
    public class GoogleLocal : IGoogleLocal, ILoginProvider
    {
        public GoogleLocal(GoogleOptions googleOptions, IMethodBinderStore methodBinderStore)
        {
            GoogleOptions = googleOptions;
            MethodBinderStore = methodBinderStore;
        }
        public string ProviderName => "Google";

        public GoogleOptions GoogleOptions { get; }

        public IMethodBinderStore MethodBinderStore { get; }
        public string ButtonHtml => $"<div class=\"g-signin2\" data-onsuccess=\"onSignIn\" data-theme=\"dark\"></div>";

        public async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
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

        public Task LoggedIn(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task LoggedOut(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith("GoogleLocal.LoginScript.js", StringComparison.InvariantCultureIgnoreCase));
            using (var res = GetType().Assembly.GetManifestResourceStream(resName))
            {
                using (var sr = new StreamReader(res))
                {
                    var script = sr.ReadToEnd();
                    
                    var enc = Encoding.UTF8;
                    return new WebContent()
                    {
                        Content = new MemoryStream(enc.GetBytes(script)),
                        ContentType = "text/html",
                        CharSet = enc.HeaderName
                    };
                }
            }
        }
    }
}
