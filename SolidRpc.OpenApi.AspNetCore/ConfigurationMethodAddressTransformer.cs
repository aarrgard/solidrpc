using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly:SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IMethodAddressTransformer), typeof(ConfigurationMethodAddressTransformer))]

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implements the uri transformer
    /// </summary>
    public class ConfigurationMethodAddressTransformer : IMethodAddressTransformer
    {
        /// <summary>
        /// The configuration variable that stores the path prefix
        /// </summary>
        public const string ConfigPathPrefix = "SolidRpc.BaseUriTransformer.PathPrefix";

        /// <summary>
        /// The configuration variable that stores the scheme
        /// </summary>
        public const string ConfigScheme = "SolidRpc.BaseUriTransformer.Scheme";

        /// <summary>
        /// The configuration variable that stores the scheme
        /// </summary>
        public const string ConfigHost = "SolidRpc.BaseUriTransformer.Host";

        /// <summary>
        /// The configuration variable that stores the variables to look for the host
        /// </summary>
        public const string ConfigHostSettings = "SolidRpc.BaseUriTransformer.HostConfigSettings";

        /// <summary>
        /// Constructs a new instance of the uri transformer.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurationMethodAddressTransformer(IConfiguration configuration)
        {
            Scheme = configuration[ConfigScheme];
            Host = HostString.FromUriComponent(configuration[ConfigHost]);
            PathPrefix = configuration[ConfigPathPrefix];

            if (string.IsNullOrEmpty(Host.Host))
            {
                var hostConfigSettings = configuration[ConfigHostSettings] ?? "urls,WEBSITE_HOSTNAME";
                if (!string.IsNullOrEmpty(hostConfigSettings))
                {
                    foreach(var hostConfigSetting in hostConfigSettings.Split(','))
                    {
                        if (!string.IsNullOrEmpty(configuration[hostConfigSetting]))
                        {
                            var hostString = configuration[hostConfigSetting]
                                .Split(';')
                                .OrderBy(o => o.StartsWith("https") ? 0 : 1)
                                .First();
                            if(hostString.StartsWith("http"))
                            {
                                if(!Uri.TryCreate(hostString, UriKind.RelativeOrAbsolute, out Uri uri))
                                {
                                    throw new Exception($"Cannot parse uri:{hostString}");
                                }
                                Scheme = uri.Scheme;
                                Host = new HostString(uri.Host, uri.Port);
                            }
                            else
                            {
                                Host = HostString.FromUriComponent(hostString);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private string Scheme { get; }
        private HostString Host { get; }
        private string PathPrefix { get; }

        Task<Uri> IMethodAddressTransformer.TransformUriAsync(Uri uri, MethodInfo methodInfo)
        {
            //
            // we are only interested in transforming the base path
            // of the open api spec. not individual methods.
            //
            if(methodInfo != null)
            {
                return Task.FromResult(uri);
            }
            var newUri = new UriBuilder(uri);
            if (!string.IsNullOrEmpty(Scheme))
            {
                newUri.Scheme = Scheme;
                if (string.Equals(Scheme, "http", StringComparison.InvariantCultureIgnoreCase))
                {
                    newUri.Port = 80;
                }
                if (string.Equals(Scheme, "https", StringComparison.InvariantCultureIgnoreCase))
                {
                    newUri.Port = 443;
                }
            }
            if (!string.IsNullOrEmpty(Host.Host))
            {
                newUri.Host = Host.Host;
                if (Host.Port != null)
                {
                    newUri.Port = Host.Port.Value;
                }
            }
            if(!string.IsNullOrEmpty(PathPrefix))
            {
                newUri.Path = $"{PathPrefix}{newUri.Path}";
            }
            return Task.FromResult(newUri.Uri);
        }
    }
}
