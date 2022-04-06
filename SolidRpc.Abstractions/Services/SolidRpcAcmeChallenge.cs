using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcServiceAttribute(typeof(ISolidRpcAcmeChallenge), typeof(SolidRpcAcmeChallenge))]
namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the logic for the acme challenge
    /// </summary>
    public class SolidRpcAcmeChallenge : ISolidRpcAcmeChallenge
    {
        private static IDictionary<string, string> s_challenges = new Dictionary<string,string>();

        public Task<FileContent> GetAcmeChallengeAsync(string key, CancellationToken cancellation = default)
        {
            if(!s_challenges.TryGetValue(key, out string challenge))
            {
                throw new FileContentNotFoundException();
            }
            var enc = Encoding.UTF8;
            return Task.FromResult(new FileContent()
            {
                Content = new MemoryStream(enc.GetBytes(challenge)),
                ContentType = "text/plain",
                CharSet = enc.HeaderName
            }); ;
        }

        public Task SetAcmeChallengeAsync(string challenge, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(challenge)) throw new ArgumentException();
            var parts = challenge.Split('.');
            if (parts.Length != 2) throw new ArgumentException();
            s_challenges[parts[0]] = challenge;
            return Task.CompletedTask;
        }
    }
}
