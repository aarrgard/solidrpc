using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IHouse {
        /// <summary>
        /// Metod f�r att uppdatera en villa.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id p� villan</param>
        /// <param name="house">Villainformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.House.HouseUpdate.NoContentException">No Content</exception>
        Task HouseUpdate(
            string customerId,
            string id,
            CmsHouse house,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att skapa en villa.&lt;br /&gt;
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