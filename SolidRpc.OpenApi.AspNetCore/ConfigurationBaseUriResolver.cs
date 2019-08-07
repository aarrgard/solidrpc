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
        /// Constructs a new instance of the uri transformer.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurationBaseUriTransformer(IConfiguration configuration)
        {
            Scheme = configuration[$"SolidRpc.BaseUriTransformer.Scheme"];
            Host = HostString.FromUriComponent(configuration[$"SolidRpc.BaseUriTransformer.Host"]);

            if(string.IsNullOrEmpty(Host.Host))
            {
                var hostConfigSettings = configuration[$"SolidRpc.BaseUriTransformer.HostConfigSettings"] ?? "urls,WEBSITE_HOSTNAME";
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
            return newUri.Uri;
        }
    }
}
