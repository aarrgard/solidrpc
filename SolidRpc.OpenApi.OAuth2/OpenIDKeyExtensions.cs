using Microsoft.IdentityModel.Tokens;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SolidRpc.Abstractions.Types.OAuth2
{
    /// <summary>
    /// Extension methods for the oauth2 key
    /// </summary>
    public static class OpenIDKeyExtensions
    {
        private static ConcurrentDictionary<string, SecurityKey> s_SecurityKeys = new ConcurrentDictionary<string, SecurityKey>();
        private static ConcurrentDictionary<string, RSACryptoServiceProvider> s_RsaKeys = new ConcurrentDictionary<string, RSACryptoServiceProvider>();

        /// <summary>
        /// Returns the key as a security key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SecurityKey AsSecurityKey(this OpenIDKey key)
        {
            var keyId = $"{key.Kid}:{key.Kty}:{key.E}:{key.N}";
            return s_SecurityKeys.GetOrAdd(keyId, _ => CreateSecurityKey(key));
        }

        /// <summary>
        /// Returns the key as a security key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider AsRSACryptoProvider(this OpenIDKey key)
        {
            var keyId = $"{key.Kid}:{key.Kty}:{key.E}:{key.N}";
            return s_RsaKeys.GetOrAdd(keyId, _ => CreateRsaKey(key));
        }

        private static RSACryptoServiceProvider CreateRsaKey(OpenIDKey key)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportParameters(
                new RSAParameters()
                {
                    Modulus = FromBase64Url(key.N),
                    Exponent = FromBase64Url(key.E),
                    D = FromBase64Url(key.D)
                }
            );
            return rsaProvider;
        }

        private static byte[] FromBase64Url(string base64Url)
        {
            if (base64Url == null) return null;
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                                  .Replace("-", "+");
            var s = Convert.FromBase64String(base64);
            return s;
        }
        private static SecurityKey CreateSecurityKey(OpenIDKey key)
        {
            key = JsonHelper.Deserialize<OpenIDKey>(JsonHelper.Serialize(key, key.GetType()));
            key.D = null;
            var sw = new StringWriter();
            JsonHelper.Serialize(sw, key, typeof(OpenIDKey));
            var json = $"{{\"Keys\":[{sw}]}}";
            var jsonKeys = JsonWebKeySet.Create(json);
            return jsonKeys.GetSigningKeys().First();
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
    }
}
