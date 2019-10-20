using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Interest.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface Interest {
        /// <summary>
        /// Skickar in intresseanm�lan fr�n en befintlig kontakt till en bostad.
        /// </summary>
        /// <param name="interest">Intresseanm�lan</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Interest.InterestSendInterestOnEstate.NoContentException">No Content</exception>
        Task InterestSendInterestOnEstate(
            InterestOnEstate interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm�lan fr�n en ny kontakt till en bostad.
        /// Innan nya personer l�ggs in i m�klarsystemet, g�rs alltid en dubblettkontroll.
        /// </summary>
        /// <param name="interest">InterestAndContact</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Interest.InterestSendInterestAndContactOnEstate.NoContentException">No Content</exception>
        Task InterestSendInterestAndContactOnEstate(
            InterestOnEstateAndContact interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm�lan till m�klarsystemet f�r ett eller flera omr�den. Skickar in intresseanm�lan till m�klarsystemet f�r ett eller flera omr�den.
        /// </summary>
        /// <param name="interest"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Interest.InterestSendInterestOnArea.NoContentException">No Content</exception>
        Task InterestSendInterestOnArea(
            InterestOnArea interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm�lan f�r ett eller flera omr�den, tillsammans med kontaktuppgifter. Innan nya personer l�ggs in i m�klarsystemet, g�rs alltid en dubblettkontroll.
        /// </summary>
        /// <param name="interest"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Interest.InterestSendInterestAndContactOnArea.NoContentException">No Content</exception>
        Task InterestSendInterestAndContactOnArea(
            InterestAndContactOnArea interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}