using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Services;
using SolidRpc.Security.Services;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Back.Services
{
    public class SolidRpcSecurity : ISolidRpcSecurity
    {
        public SolidRpcSecurity(
            IServiceProvider serviceProvider, 
            IMethodBinderStore methodBinderStore,
            ISolidRpcContentHandler contentHandler)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            ContentHandler = contentHandler ?? throw new ArgumentNullException(nameof(contentHandler));
        }

        private IServiceProvider ServiceProvider { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISolidRpcContentHandler ContentHandler { get; }

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

        public Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
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
            var httpInvoker = ServiceProvider.GetRequiredService<IInvoker<ISolidRpcSecurity>>();
            var baseScript = await httpInvoker.GetUriAsync(o => o.LoginScript(cancellationToken));
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            var providers = await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
            return providers.SelectMany(o => o.Script).Union(new[] {
                baseScript
            });
        }

        public Task<IEnumerable<Claim>> Profile(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IEnumerable<Claim>>(new[] {
                new Claim() {Name = "test", Value = "test"},
                new Claim() {Name = "test2", Value = "test2"},
            });
        }
    }
}
