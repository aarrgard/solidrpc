using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IForeignProperty {
        /// <summary>
        /// Metod f�r att uppdatera ett utlandsobjekt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id p� utlandsobjektet</param>
        /// <param name="foreignProperty">Informationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.ForeignProperty.ForeignPropertyUpdate.NoContentException">No Content</exception>
        Task ForeignPropertyUpdate(
            string customerId,
            string id,
            CmsForeignProperty foreignProperty,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att skapa ett utlandsobjekt.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="foreignProperty">Informationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        Task<string> ForeignPropertyCreate(
            string customerId,
            CmsForeignProperty foreignProperty,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}