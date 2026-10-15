
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Extension methods for the oauth2 extensions
    /// </summary>
    public static class IMethodAddressTransformerExtensions
    {
        /// <summary>
        /// Returns the local issuer for the oauth2 authority
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static string GetSolidRpcOAuth2LocalIssuer(this IMethodAddressTransformer mat)
        {
            var baseAddress = mat.BaseAddress.ToString();
            if (baseAddress.EndsWith("/")) baseAddress = baseAddress.Substring(0, baseAddress.Length - 1);
            var oauth2Issuer = $"{baseAddress}/SolidRpc/Abstractions";
            return oauth2Issuer;
        }

    }
}
