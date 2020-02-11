using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Advertising.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IAdvertisingEstate {
        /// <summary>
        /// Skickar in en ny intresseanm&#228;lan f&#246;r en kontakt till en bostad. Innan nya personer l&#228;ggs in i m&#228;klarsystemet, g&#246;rs alltid en dubblettkontroll.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="estateId">Id för aktuell bostad</param>
        /// <param name="application">Uppgifter för intresseanmälan</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.AdvertisingEstate.AdvertisingEstateInterestApplication.NoContentException">No Content</exception>
        Task AdvertisingEstateInterestApplication(
            string customerId,
            string estateId,
            AdvertisingInterestApplication application,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}