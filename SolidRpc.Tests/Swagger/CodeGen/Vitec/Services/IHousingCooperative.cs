using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IHousingCooperative {
        /// <summary>
        /// Metod f�r att uppdatera en bostadsr�tt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id p� bostadsr�tten</param>
        /// <param name="housingCooperative">Bostadsr�ttsinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.HousingCooperative.HousingCooperativeUpdate.NoContentException">No Content</exception>
        Task HousingCooperativeUpdate(
            string customerId,
            string id,
            CmsHousingCooperative housingCooperative,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att skapa en bostadsr�tt.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="housingCooperative">Bostadsr�ttsinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        Task<string> HousingCooperativeCreate(
            string customerId,
            CmsHousingCooperative housingCooperative,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}