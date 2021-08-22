using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Services;
using System;
using System.Text;

[assembly: SolidRpcService(typeof(ISolidRpcContinuationToken), typeof(SolidRpcContinuationToken), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Services
{
    /// <summary>
    /// Represents a continuation token. This token is added as a header to the
    /// responses and parsed for the requests
    /// </summary>
    public class SolidRpcContinuationToken : ISolidRpcContinuationToken
    {
        public SolidRpcContinuationToken(ISerializerFactory serializerFactory)
        {
            SerializerFactory = serializerFactory;
        }

        private ISerializerFactory SerializerFactory { get; }

        /// <summary>
        /// The token in string representation.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Returns the token 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetToken<T>()
        {
            if(string.IsNullOrEmpty(Token))
            {
                return default(T);
            }
            var strToken = Encoding.UTF8.GetString(Convert.FromBase64String(Token));
            SerializerFactory.DeserializeFromString(strToken, out T token);
            return token;
        }

        /// <summary>
        /// Sets the continuation token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        public void SetToken<T>(T token)
        {
            SerializerFactory.SerializeToString(out string strToken, token);
            Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(strToken));
        }
    }
}
