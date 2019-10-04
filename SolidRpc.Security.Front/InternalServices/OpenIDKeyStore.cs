using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SolidRpc.Security.Front.InternalServices
{
    public class OpenIDKeyStore : IOpenIDKeyStore
    {
        private class SigningKey
        {
            public SigningKey()
            {
                var keyId = Guid.NewGuid().ToString();
                Created = DateTime.Now;
                var rsa = RSA.Create();
                PrivateKey = new RsaSecurityKey(rsa.ExportParameters(true))
                {
                    KeyId = keyId
                };
                PublicKey = new RsaSecurityKey(rsa.ExportParameters(false))
                {
                    KeyId = keyId
                };
            }
            public string KeyId => PublicKey.KeyId;
            public DateTime Created { get; }
            public RsaSecurityKey PublicKey { get; }
            public RsaSecurityKey PrivateKey { get; }
        }
        public OpenIDKeyStore()
        {
            SigningKeys = new ConcurrentDictionary<string, SigningKey>();
            var tmp = CreateNewSigningKey().Result;
        }

        private IDictionary<string, SigningKey> SigningKeys { get; }
        private SigningKey CurrentSigningKey { get; set; }

        public Task<string> CreateNewSigningKey(CancellationToken cancellationToken = default(CancellationToken))
        {
            var newKey = new SigningKey();
            SigningKeys[newKey.KeyId] = newKey;
            CurrentSigningKey = newKey;
            return Task.FromResult(newKey.KeyId);
        }

        public Task<RsaSecurityKey> GetSigningPrivateKey(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(CurrentSigningKey.PrivateKey);
        }

        public Task<IEnumerable<RsaSecurityKey>> GetSigningPublicKeys(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IEnumerable<RsaSecurityKey>>(SigningKeys.Values.Select(o => o.PublicKey));
        }
    }
}
