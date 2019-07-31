using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using System;

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
            var hostConfigSetting = configuration[$"SolidRpc.BaseUriTransformer.HostConfigSetting"];
            if (!string.IsNullOrEmpty(hostConfigSetting))
            {
                HostFromConfig = HostString.FromUriComponent(configuration[hostConfigSetting]);
            }
        }

        private string Scheme { get; }
        private HostString Host { get; }
        private HostString HostFromConfig { get; }

        Uri IBaseUriTransformer.TransformUri(Uri uri)
        {
            var newUri = new UriBuilder(uri);
            if (!string.IsNullOrEmpty(Scheme))
            {
                newUri.Scheme = Scheme;
            }
            if (!string.IsNullOrEmpty(HostFromConfig.Host))
            {
                SetHostAndPort(newUri, HostFromConfig);
            }
            if (!string.IsNullOrEmpty(Host.Host))
            {
                SetHostAndPort(newUri, Host);
            }
            return newUri.Uri;
        }

        private void SetHostAndPort(UriBuilder newUri, HostString host)
        {
            newUri.Host = host.Host;
            if(host.Port != null)
            {
                newUri.Port = host.Port.Value;
            }
        }
    }
}
