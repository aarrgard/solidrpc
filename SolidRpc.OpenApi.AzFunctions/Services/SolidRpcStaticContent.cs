using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;

[assembly: SolidRpcAbstractionProvider(typeof(ISolidRpcStaticContent), typeof(SolidRpcStaticContent))]

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// Contains the static content
    /// </summary>
    public class SolidRpcStaticContentAz : SolidRpcStaticContent
    {
    }
}
