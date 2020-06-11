using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Contains extensions for the security keys.
    /// </summary>
    public class SecurityKeyExtensions
    {
        /// <summary>
        /// Checks the security key for the invocation.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="secKey"></param>
        /// <param name="keyFetcher"></param>
        /// <param name="cancellatinToken"></param>
        /// <returns></returns>
        public static Task CheckSecurityKeyAsync(
            object caller,
            KeyValuePair<string, string>? secKey,
            Func<string, string> keyFetcher,
            CancellationToken cancellatinToken = default(CancellationToken))
        {
            if (secKey == null)
            {
                return Task.CompletedTask;
            }

            var key = secKey.Value.Key.ToLower();
            var value = secKey.Value.Value;
            // calls invoked directly from a proxy are allowed
            if (caller is ISolidProxy)
            {
                return Task.CompletedTask;
            }

            //
            // check the security key if specified
            //
            var callKey = keyFetcher(key);
            if (string.IsNullOrEmpty(callKey))
            {
                throw new UnauthorizedException($"No '{key}' specified");
            }
            if (!value.Equals(callKey.ToString()))
            {
                throw new UnauthorizedException("SolidRpcSecurityKey differs");
            }
            return Task.CompletedTask;

        }
    }
}
