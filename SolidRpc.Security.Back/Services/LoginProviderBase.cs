using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Back.Services
{
    public abstract class LoginProviderBase : ILoginProvider
    {

        public static Task<WebContent> GetManifestResourceAsWebContent(string resourceName, IEnumerable<KeyValuePair<string, string>> replace = null)
        {
            var contentType = "application/octet-stream";
            if (resourceName.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase))
            {
                contentType = "text/javascript";
            }
            else if (resourceName.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase))
            {
                contentType = "text/html";
            }
            var resName = typeof(LoginProviderBase).Assembly.GetManifestResourceNames().Single(o => o.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase));
            using (var res = typeof(LoginProviderBase).Assembly.GetManifestResourceStream(resName))
            {
                using (var sr = new StreamReader(res))
                {
                    var script = sr.ReadToEnd();

                    if (replace != null)
                    {
                        foreach (var kv in replace)
                        {
                            script = script.Replace(kv.Key, kv.Value);
                        }
                    }

                    var enc = Encoding.UTF8;
                    return Task.FromResult(new WebContent()
                    {
                        Content = new MemoryStream(enc.GetBytes(script)),
                        ContentType = contentType,
                        CharSet = enc.HeaderName
                    });
                }
            }
        }

        public abstract string ProviderName { get; }

        public abstract Task<LoginProvider> LoginProvider(CancellationToken cancellationToken);
    }
}
