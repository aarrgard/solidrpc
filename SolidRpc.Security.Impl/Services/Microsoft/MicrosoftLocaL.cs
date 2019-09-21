using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Microsoft;
using SolidRpc.Security.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.Microsoft
{
    public class MicrosoftLocal : LoginProviderBase, IMicrosoftLocal
    {
        public MicrosoftLocal(IMethodBinderStore methodBinderStore)
        {
            MethodBinderStore = methodBinderStore;
        }
        public override string ProviderName => "Microsoft";

        public IMethodBinderStore MethodBinderStore { get; }
        public string ButtonHtml => $"<img src=\"{ProviderName}\" alt=\"{ProviderName}\"/>";

        public override async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new LoginProvider()
            {
                Name = ProviderName,
                Meta = new LoginProviderMeta[0],
                Script = new[] { await MethodBinderStore.GetUrlAsync<IMicrosoftLocal>(o => o.LoginScript(cancellationToken)) },
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
            return GetManifestResourceAsWebContent("MicrosoftLocal.LoginScript.js");
        }
    }
}
