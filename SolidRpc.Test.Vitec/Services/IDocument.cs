using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Documents.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IDocument {
        /// <summary>
        /// Metod f&#246;r att l&#228;gga till eller uppdatera ett  dokument till en bostad.
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