using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.DateParam.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IDateParam {
        /// <summary>
        /// Sends a date back and forth between client and server
        /// </summary>
        /// <param name="p">The number to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<DateTime> ProxyDateInParam(
            DateTime p,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}