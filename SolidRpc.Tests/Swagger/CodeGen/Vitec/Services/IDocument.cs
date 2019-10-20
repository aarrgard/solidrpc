using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Documents.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IDocument {
        /// <summary>
        /// Metod f�r att l�gga till eller uppdatera ett  dokument till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        Task<string> DocumentPost(
            string customerId,
            string estateId,
            Document document,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}