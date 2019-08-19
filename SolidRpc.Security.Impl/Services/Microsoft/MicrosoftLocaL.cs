using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services.Microsoft;
using SolidRpc.Security.Types;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services.Microsoft
{
    public class MicrosoftLocal : IMicrosoftLocal, ILoginProvider
    {
        public MicrosoftLocal(IMethodBinderStore methodBinderStore)
        {
            MethodBinderStore = methodBinderStore;
        }
        public string ProviderName => "Microsoft";

        public IMethodBinderStore MethodBinderStore { get; }
        public string ButtonHtml => $"<img src=\"{ProviderName}\" alt=\"{ProviderName}\"/>";

        public async Task<LoginProvider> LoginProvider(CancellationToken cancellationToken = default(CancellationToken))
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
            var html = @"<html></html>";
            var enc = Encoding.UTF8;
            return Task.FromResult(new WebContent()
            {
                Content = new MemoryStream(enc.GetBytes(html)),
                ContentType = "text/html",
                CharSet = enc.HeaderName
            });
        }
    }
}
