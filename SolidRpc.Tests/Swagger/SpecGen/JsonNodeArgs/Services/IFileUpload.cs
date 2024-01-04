using SolidRpc.Tests.Swagger.SpecGen.JsonNodeArgs.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.JsonNodeArgs.Services
{
    /// <summary>
    /// Test interface to upload a file
    /// </summary>
    public interface IJsonNodeArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonNode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<JsonNode> ProxyJsonNodeAsync(JsonNode jsonNode, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonNode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ComplexType> ProxyComplexTypeAsync(ComplexType jsonNode, CancellationToken cancellationToken = default);
    }
}
