using SolidRpc.Abstractions.Types.RateLimit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services.RateLimit
{
    /// <summary>
    /// Service that we can invoke to throttle requests.
    /// </summary>
    public interface ISolidRpcRateLimit
    {
        /// <summary>
        /// Returns the rate limit token.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RateLimitToken> GetRateLimitTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the singelton token for supplied key. This call implies a rate limit setting of max 1 concurrent call.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RateLimitToken> GetSingeltonTokenAsync(string resourceName, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns a rate limit token.
        /// </summary>
        /// <param name="rateLimitToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ReturnRateLimitTokenAsync(RateLimitToken rateLimitToken, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the rate limit settings
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<RateLimitSetting>> GetRateLimitSettingsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates the rate limit settings
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateRateLimitSetting(RateLimitSetting setting, CancellationToken cancellationToken = default(CancellationToken));
    }
}
