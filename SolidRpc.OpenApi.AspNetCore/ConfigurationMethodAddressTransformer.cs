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
        private static readonly string BaseConfig = "SolidRpc:AddressTransformer";
        /// <summary>
        /// The configuration variable that stores the path prefix
        /// </summary>
        public static readonly string[] ConfigPathPrefix = new [] { $"{BaseConfig}:PathPrefix", "SolidRpc.BaseUriTransformer.PathPrefix" };

        /// <summary>
        /// The configuration variable that stores the scheme
        /// </summary>
        public static readonly string[] ConfigScheme = new[] { $"{BaseConfig}:Scheme", "SolidRpc.BaseUriTransformer.Scheme" };

        /// <summary>
        /// The allowed CORS origins
        /// </summary>
        public static readonly string[] ConfigCors = new[] { $"{BaseConfig}:Cors", "SolidRpc.BaseUriTransformer.Cors" };

        /// <summary>
        /// The configuration variable that stores the host
        /// </summary>
        public static readonly string[] ConfigHost = new[] { $"{BaseConfig}:Host", "SolidRpc.BaseUriTransformer.Host" };

        /// <summary>
        /// The configuration variable that stores the variables to look for the host
        /// </summary>
        public static readonly string[] ConfigHostSettings = new[] { $"{BaseConfig}:HostConfigSettings", "SolidRpc.BaseUriTransformer.HostConfigSettings" };

        /// <summary>
        /// The configuration variable that specifies the rewrites that we should do.
        /// </summary>
        public static readonly string[] ConfigPathRewrites = new[] { $"{BaseConfig}:PrefixRewrites" };

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
                Scheme = GetValue(configuration, ConfigScheme);
                Host = HostString.FromUriComponent(GetValue(configuration,ConfigHost));
                PathPrefix = GetValue(configuration, ConfigPathPrefix);

                if (string.IsNullOrEmpty(Host.Host))
                {
                    var hostConfigSettings = GetValue(configuration,ConfigHostSettings,"urls,WEBSITE_HOSTNAME");
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
            BaseAddress = TransformUri(new Uri("https://localhost/"), null);

            ConfiguredCors = GetValue(configuration,ConfigCors,"").Split(',')
                .Where(o => !string.IsNullOrEmpty(o))
                .Distinct()
                .ToList();
            PathRewrites = ParsePathRewrites(GetValue(configuration, ConfigPathRewrites, ""));
        }

        private string[][] ParsePathRewrites(string rewrites)
        {
            var retVal = new List<string[]>();
            foreach (var part in rewrites.Split(','))
            {
                if (string.IsNullOrEmpty(part)) continue;
                var parts = part.Split(':').Select(o => o.Trim()).ToArray();
                if (parts.Length != 2) throw new Exception("Invalid rewrite rule:" + part);
                if(parts[0] != parts[1])
                {
                    retVal.Add(parts);
                }
            }

            return retVal.ToArray();
        }

        private string GetValue(IConfiguration configuration, string[] confNames, string defaultValue = null)
        {
            foreach(var confName in confNames)
            {
                var value = configuration[confName];
                if(value != null)
                {
                    return value;
                }
            }
            return defaultValue;
        }

        public IEnumerable<string> ConfiguredCors { get; set; }
        private string Scheme { get; }
        private HostString Host { get; }
        private string PathPrefix { get; }
        private string[][] PathRewrites { get; }

        /// <summary>
        /// The  base address
        /// </summary>
        public Uri BaseAddress { get; set; }

        /// <summary>
        /// Returs the allowed origins.
        /// </summary>
        public IEnumerable<string> AllowedCorsOrigins
        {
            get
            {
                return ConfiguredCors.Union(new[] { ConvertToCorsOrigin(BaseAddress) });
            }
        }

        private string ConvertToCorsOrigin(Uri baseAddress)
        {
            if(baseAddress.IsDefaultPort)
            {
                return $"{baseAddress.Scheme}://{baseAddress.Host}";
            }
            else
            {
                return $"{baseAddress.Scheme}://{baseAddress.Host}:{baseAddress.Port}";
            }
        }

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

        public string RewritePath(string path)
        { 
            for(int i = 0; i < 100; i++)
            {
                var match = PathRewrites.FirstOrDefault(o => path.StartsWith(o[0]));
                if (match == null) return path;
                if (path == match[0]) return match[1];
                if (path[match[0].Length] != '/') return path;

                path = $"{match[1]}{path.Substring(match[0].Length)}";
            }
            throw new Exception("Followed more than 100 rewrites - invalid configuration");
        }
    }
}
