using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using System;
using System.Linq;

[assembly:SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IBaseUriTransformer), typeof(ConfigurationBaseUriTransformer))]

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implements the uri transformer
    /// </summary>
    public class ConfigurationBaseUriTransformer : IBaseUriTransformer
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
        public ConfigurationBaseUriTransformer(IConfiguration configuration)
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
                                Scheme = "http";
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

        Uri IBaseUriTransformer.TransformUri(Uri uri)
        {
            var newUri = new UriBuilder(uri);
            if (!string.IsNullOrEmpty(Scheme))
            {
                newUri.Scheme = Scheme;
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
            return newUri.Uri;
        }
    }
}
