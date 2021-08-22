using System;
using System.Threading;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Represents a continuation token.
    /// </summary>
    public interface ISolidRpcContinuationToken
    {
        /// <summary>
        /// The continuation token
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Returns the token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetToken<T>();

        /// <summary>
        /// Returns the token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        void SetToken<T>(T token);
    }

    /// <summary>
    /// Extension methods for the token
    /// </summary>
    public static class ISolidRpcContinuationTokenExtensions
    {
        /// <summary>
        /// The continuation token
        /// </summary>
        public static readonly AsyncLocal<ISolidRpcContinuationToken> ContinuationToken = new AsyncLocal<ISolidRpcContinuationToken>();

        private class SolidRpcContinuationTokenScope : IDisposable
        {
            private ISolidRpcContinuationToken _parent;
            public SolidRpcContinuationTokenScope(ISolidRpcContinuationToken parent)
            {
                _parent = parent;
            }
            public void Dispose()
            {
                ContinuationToken.Value = _parent;
            }
        }

        /// <summary>
        /// Returns the http header name for the continuation token
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        public static string GetHttpHeaderName(this ISolidRpcContinuationToken continuationToken)
        {
            return "X-SolidRpc-ContinuationToken";
        }
        
        /// <summary>
        /// Pushes the token
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        public static IDisposable PushToken(this ISolidRpcContinuationToken continuationToken)
        {
            var x = new SolidRpcContinuationTokenScope(ContinuationToken.Value);
            ContinuationToken.Value = continuationToken;
            return x;
        }

        /// <summary>
        /// Pushes the token
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        public static ISolidRpcContinuationToken LoadToken(this ISolidRpcContinuationToken continuationToken)
        {
            var token = ContinuationToken.Value;
            if(continuationToken != null)
            {
                continuationToken.Token = token?.Token;
            }
            return token;
        }
    }
}
