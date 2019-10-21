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
        /// Metod f&#246;r att uppdatera en bostadsr&#228;tt.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på bostadsrätten</param>
        /// <param name="housingCooperative">Bostadsrättsinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.HousingCooperative.HousingCooperativeUpdate.NoContentException">No Content</exception>
        Task HousingCooperativeUpdate(
            string customerId,
            string id,
            CmsHousingCooperative housingCooperative,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa en bostadsr&#228;tt.&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="housingCooperative">Bostadsrättsinformationen som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        Task<string> HousingCooperativeCreate(
            string customerId,
            CmsHousingCooperative housingCooperative,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}