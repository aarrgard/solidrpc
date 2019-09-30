using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Back.InternalServices
{
    /// <summary>
    /// The keystore contains all the active keys.
    /// </summary>
    public interface IOpenIDKeyStore
    {
        /// <summary>
        /// Creates a new signing key. 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The keyid of the created key.</returns>
        Task<string> CreateNewSigningKey(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the public keys used for signing(written in the public key document).
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<RsaSecurityKey>> GetSigningPublicKeys(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the private keys used for signing(used to sign new tokens).
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RsaSecurityKey> GetSigningPrivateKey(CancellationToken cancellationToken = default(CancellationToken));
    }
}
