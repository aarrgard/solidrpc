using Microsoft.IdentityModel.Tokens;
using SolidRpc.OpenApi.Binder;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace SolidRpc.Abstractions.Types.OAuth2
{
    /// <summary>
    /// Extension methods for the oauth2 key
    /// </summary>
    public static class OpenIDKeyExtensions
    {
        private static ConcurrentDictionary<string, SecurityKey> s_SecurityKeys = new ConcurrentDictionary<string, SecurityKey>();

        /// <summary>
        /// Adds the oauth2 services
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SecurityKey AsSecurityKey(this OpenIDKey key)
        {
            var keyId = $"{key.Kid}:{key.Kty}:{key.E}:{key.N}";
            return s_SecurityKeys.GetOrAdd(keyId, _ => CreateKey(key));
        }

        /// <summary>
        /// Adds the oauth2 services
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static OpenIDKey AsOpenIDKey(this SecurityKey key)
        {
            var k = JsonWebKeyConverter.ConvertFromSecurityKey(key);
            var ms = JsonHelper.Serialize(k, typeof(JsonWebKey));
            return JsonHelper.Deserialize<OpenIDKey>(ms);
        }

        private static SecurityKey CreateKey(OpenIDKey key)
        {
            var sw = new StringWriter();
            JsonHelper.Serialize(sw, key, typeof(OpenIDKey));
            var json = $"{{\"Keys\":[{sw}]}}";
            var jsonKeys = JsonWebKeySet.Create(json);
            return jsonKeys.GetSigningKeys().First();
        }
    }
}
