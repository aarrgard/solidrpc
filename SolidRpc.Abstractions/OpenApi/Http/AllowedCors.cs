using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Http;
using System.Collections.Generic;

[assembly: SolidRpcService(typeof(AllowedCors), typeof(AllowedCors), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Abstractions.OpenApi.Http
{
    /// <summary>
    /// The allowed cors
    /// </summary>
    public class AllowedCors
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public AllowedCors()
        {
            Origins = new[] { "localhost" };
        }

        /// <summary>
        /// All the origins.
        /// </summary>
        public IEnumerable<string> Origins { get; set; }
    }
}
