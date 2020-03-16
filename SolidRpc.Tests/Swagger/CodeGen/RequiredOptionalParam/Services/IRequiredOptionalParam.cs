using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.RequiredOptionalParam.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IRequiredOptionalParam {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="p">The number to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> RequiredInt(
            int p,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="p">The number to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> OptionalInt(
            int? p = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}