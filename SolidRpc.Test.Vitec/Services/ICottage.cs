using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICottage {
        /// <summary>
        /// Metod f�r att uppdatera ett fritidshus.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id p� fritidshuset</param>
        /// <param name="cottage">Fritidshusinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Cottage.CottageUpdate.NoContentException">No Content</exception>
        Task CottageUpdate(
            string customerId,
            string id,
            CmsCottage cottage,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att skapa ett fritidshus.&lt;br /&gt;
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