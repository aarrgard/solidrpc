using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IHouse {
        /// <summary>
        /// Metod f&#246;r att uppdatera en villa.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på villan</param>
        /// <param name="house">Villainformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.House.HouseUpdate.NoContentException">No Content</exception>
        Task HouseUpdate(
            string customerId,
            string id,
            CmsHouse house,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa en villa.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="house">Villainformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        Task<string> HouseCreate(
            string customerId,
            CmsHouse house,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}