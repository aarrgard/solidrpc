using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Local.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMvcServerTest {
        /// <summary>
        /// Returns data for the index page.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task Index(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}