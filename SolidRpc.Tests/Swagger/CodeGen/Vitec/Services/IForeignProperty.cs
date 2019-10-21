using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IForeignProperty {
        /// <summary>
        /// Metod f&#246;r att uppdatera ett utlandsobjekt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på utlandsobjektet</param>
        /// <param name="foreignProperty">Informationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.ForeignProperty.ForeignPropertyUpdate.NoContentException">No Content</exception>
        Task ForeignPropertyUpdate(
            string customerId,
            string id,
            CmsForeignProperty foreignProperty,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa ett utlandsobjekt.&lt;br /&gt;
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