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
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task<WebContent> LoginPage(CancellationToken cancellationToken = default(CancellationToken))
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith("SolidRpcSecurity.LoginPage.html", StringComparison.InvariantCultureIgnoreCase));
            using (var res = GetType().Assembly.GetManifestResourceStream(resName))
            {
                using (var sr = new StreamReader(res))
                {
                    var script = sr.ReadToEnd();

                    var loginProviders = await LoginProviders(cancellationToken);
                    script = script.Replace("<!-- login meta-->", string.Join("\r\n", loginProviders.SelectMany(o => o.Meta).Select(o => $"<meta name=\"{o.Name}\" content=\"{o.Content}\"></script>")));
                    script = script.Replace("<!-- login scripts-->", string.Join("\r\n", loginProviders.SelectMany(o => o.Script).Select(o => $"<script src=\"{o}\"></script>")));
                    script = script.Replace("<!-- login buttons-->", string.Join("\r\n", loginProviders.Select(o => o.ButtonHtml)));

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

        public async Task<IEnumerable<LoginProvider>> LoginProviders(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            return await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
        }

        public async Task<IEnumerable<Uri>> LoginScripts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            var providers = await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
            return providers.SelectMany(o => o.Script);
        }
        
    }
}
