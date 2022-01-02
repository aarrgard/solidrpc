using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[assembly: SolidRpcService(typeof(IMethodAddressTransformer), typeof(ConfigurationMethodAddressTransformer), SolidRpcServiceLifetime.Singleton)]

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
        /// The configuration variable that stores the host
        /// </summary>
        public const string ConfigHost = "SolidRpc.BaseUriTransformer.Host";

        /// <summary>
        /// The configuration variable that stores the variables to look for the host
        /// </summary>
        public const string ConfigHostSettings = "SolidRpc.BaseUriTransformer.HostConfigSettings";

        /// <summary>
        /// Constructs a new instance of the uri transformer.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ConfigurationMethodAddressTransformer(IServiceProvider serviceProvider)
        {
            var baseAddress = (Uri)serviceProvider.GetService(typeof(Uri));
            var configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            if (baseAddress != null)
            {
                Scheme = baseAddress.Scheme;
                Host = new HostString(baseAddress.Host, baseAddress.Port);
                PathPrefix = baseAddress.AbsolutePath;
                if (PathPrefix.EndsWith("/")) PathPrefix = PathPrefix.Substring(0, PathPrefix.Length - 1);
            }
            else
            {
                Scheme = configuration[ConfigScheme];
                Host = HostString.FromUriComponent(configuration[ConfigHost]);
                PathPrefix = configuration[ConfigPathPrefix];

                if (string.IsNullOrEmpty(Host.Host))
                {
                    var hostConfigSettings = configuration[ConfigHostSettings] ?? "urls,WEBSITE_HOSTNAME";
                    if (!string.IsNullOrEmpty(hostConfigSettings))
                    {
                        foreach (var hostConfigSetting in hostConfigSettings.Split(','))
                        {
                            if (!string.IsNullOrEmpty(configuration[hostConfigSetting]))
                            {
                                var hostString = configuration[hostConfigSetting]
                                    .Split(';')
                                    .OrderBy(o => o.StartsWith("https") ? 0 : 1)
                                    .First();
                                if (hostString.StartsWith("http"))
                                {
                                    if (!Uri.TryCreate(hostString, UriKind.RelativeOrAbsolute, out Uri uri))
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
            BaseAddress = TransformUri(new Uri("http://localhost/"), null);
        }

        /// <summary>
        /// The  base address
        /// </summary>
        public Uri BaseAddress { get; }

        /// <summary>
        /// Returs the origins.
        /// </summary>
        public IEnumerable<string> Origins
        {
            get
            {
                var uri = BaseAddress.ToString();
                var slashIdx = uri.IndexOf('/', "https://x".Length);
                return new[] { uri.Substring(0, slashIdx)};
            }
        }

        private string Scheme { get; }
        private HostString Host { get; }
        private string PathPrefix { get; }

        /// <summary>
        /// Transforms the supplied uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public Uri TransformUri(Uri uri, MethodInfo methodInfo)
        {
            //
            // we are only interested in transforming the base path
            // of the open api spec. not individual methods.
            //
            if(methodInfo != null)
            {
                return uri ?? throw new Exception("Uri is null!");
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
            return newUri.Uri ?? throw new Exception("Uri is null!");
        }
    }
}
