using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Interfaces that defines the logic for OAuth2 support.
    /// </summary>
    public interface ISolidRpcOAuth2
    {
        /// <summary>
        /// This is the method returns a html page that calls supplied callback
        /// after the token callback has been invoked. Use this method to
        /// retreive tokens from a standalone node instance.
        /// 
        /// Start a local http server and supply the address to the handler.
        /// </summary>
        /// <returns></returns>
        Task<FileContent> GetAuthorizationCodeTokenAsync(
            Uri callbackUri = null,
            string state = null,
            IEnumerable<string> scopes = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// This is the method that is invoked when a user has been authenticated
        /// and a valid token is supplied.
        /// </summary>
        /// <returns></returns>
        Task<FileContent> TokenCallbackAsync(string code = null, string state = null, CancellationToken cancellation = default);
    }
}
