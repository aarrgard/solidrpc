using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Cms.Estate;
using System.Threading;
using SolidRpc.Test.Vitec.Types.HousingCooperative.Cms;
namespace SolidRpc.Test.Vitec.Services {
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
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.HousingCooperative.HousingCooperativeUpdate.NoContentException">No Content</exception>
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
    
        /// <summary>
        /// Metod f&#246;r att uppdatera en bostadsr&#228;tt&lt;br /&gt;
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Id på villan</param>
        /// <param name="housingCooperative">Information om bostadsrätten som ska uppdateras</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.HousingCooperative.HousingCooperativeUpdateHousingCooperativeFromSellerInput.NoContentException">No Content</exception>
        Task HousingCooperativeUpdateHousingCooperativeFromSellerInput(
            string customerId,
            string id,
            HousingCooperativeSellerInput housingCooperative,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}