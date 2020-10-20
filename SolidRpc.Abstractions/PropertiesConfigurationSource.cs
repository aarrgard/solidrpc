using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class PropertiesConfigurationSource : Dictionary<string, string>, IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigurationProvider(this);
        }

        private class ConfigurationProvider : IConfigurationProvider
        {
            public ConfigurationProvider(PropertiesConfigurationSource propertiesConfigurationSources)
            {
                PropertiesConfigurationSources = propertiesConfigurationSources;
            }

            public PropertiesConfigurationSource PropertiesConfigurationSources { get; }

            public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
            {
                return PropertiesConfigurationSources.Keys;
            }

            public IChangeToken GetReloadToken()
            {
                return new CancellationChangeToken(CancellationToken.None);
            }

            public void Load()
            {
            }

            public void Set(string key, string value)
            {
                PropertiesConfigurationSources[key] = value;
            }

            public bool TryGet(string key, out string value)
            {
                return PropertiesConfigurationSources.TryGetValue(key, out value);
            }
        }
    }
}