using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.TwoOpsOneMeth.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ITwoOpsOneMeth {
        /// <summary>
        /// This is the get version
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cancellationToken"></param>
        Task<int> TwoOpsOneMeth(
            int p,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}