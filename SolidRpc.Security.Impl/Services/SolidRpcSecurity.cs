using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Services;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services
{
    public class SolidRpcSecurity : ISolidRpcSecurity
    {
        public SolidRpcSecurity()
        {
        }

        public SolidRpcSecurity(IServiceProvider serviceProvider, IMethodBinderStore methodBinderStore)
        {
            ServiceProvider = serviceProvider;
            MethodBinderStore = methodBinderStore;
        }

        private IServiceProvider ServiceProvider { get; }
        private IMethodBinderStore MethodBinderStore { get; }

        public async Task<WebContent> LoginPage(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = await LoginProviders(cancellationToken);
            return await LoginProviderBase.GetManifestResourceAsWebContent("SolidRpcSecurity.LoginPage.html", new Dictionary<string, string>()
            {
                {"<!-- login meta-->",string.Join("\r\n", loginProviders.SelectMany(o => o.Meta).Select(o => $"<meta name=\"{o.Name}\" content=\"{o.Content}\"></script>")) },
                {"<!-- login scripts-->", string.Join("\r\n", loginProviders.SelectMany(o => o.Script).Select(o => $"<script src=\"{o}\"></script>")) },
                {"<!-- login buttons-->", string.Join("\r\n", loginProviders.Select(o => o.ButtonHtml)) },
            });
        }

        public async Task<IEnumerable<LoginProvider>> LoginProviders(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            return await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
        }

        public Task<WebContent> LoginScript(CancellationToken cancellationToken = default)
        {
            var binder = MethodBinderStore.GetMethodBinding<ISolidRpcSecurity>(o => o.LoginProviders(cancellationToken));
            var auth = binder.MethodBinder.OpenApiSpec.BaseAddress;
            return LoginProviderBase.GetManifestResourceAsWebContent("SolidRpcSecurity.LoginScript.js", new Dictionary<string, string>()
            {
                { "{oidc-client-authority}", auth.ToString() },
                { "{oidc-client-client_id}", ""},
                { "{oidc-client-redirect_uri}", ""},
            });
        }

        public async Task<IEnumerable<Uri>> LoginScripts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var baseScript = await MethodBinderStore.GetUrlAsync<ISolidRpcSecurity>(o => o.LoginScript(cancellationToken));
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            var providers = await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
            return providers.SelectMany(o => o.Script).Union(new[] {
                baseScript
            });
        }

        public Task<IEnumerable<Claim>> Profile(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
