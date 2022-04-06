using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Provides logic for the acme challange
    /// </summary>
    public interface ISolidRpcAcmeChallenge
    {
        /// <summary>
        /// Sets the acme challenge. The part before the . is the filename and all of the 
        /// supplied challenge is provided in the file.
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SetAcmeChallengeAsync(
            string challenge, 
            CancellationToken cancellation = default);

        /// <summary>
        /// Returns the acme challenge
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [OpenApi(Path = "/.well-known/acme-challenge/{key}")]
        Task<FileContent> GetAcmeChallengeAsync(
            string key,
            CancellationToken cancellation = default);
    }
}
