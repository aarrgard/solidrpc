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
        Task<int> TwoOpsOneMethGet(
            int p,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// This is the post version
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cancellationToken"></param>
        Task<int> TwoOpsOneMethPost(
            int p,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}