using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICottage {
        /// <summary>
        /// Metod f&#246;r att uppdatera ett fritidshus.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på fritidshuset</param>
        /// <param name="cottage">Fritidshusinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Cottage.CottageUpdate.NoContentException">No Content</exception>
        Task CottageUpdate(
            string customerId,
            string id,
            CmsCottage cottage,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa ett fritidshus.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="cottage">Fritidshusinformationen som ska sparas</param>
        /// <param name="cancellationToken"></param>
        Task<string> CottageCreate(
            string customerId,
            CmsCottage cottage,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}