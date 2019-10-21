using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface Interest {
        /// <summary>
        /// Skickar in intresseanm&#228;lan fr&#229;n en befintlig kontakt till en bostad.
        /// </summary>
        /// <param name="interest">Intresseanmälan</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Interest.InterestSendInterestOnEstate.NoContentException">No Content</exception>
        Task InterestSendInterestOnEstate(
            InterestOnEstate interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm&#228;lan fr&#229;n en ny kontakt till en bostad.
        /// Innan nya personer l&#228;ggs in i m&#228;klarsystemet, g&#246;rs alltid en dubblettkontroll.
        /// </summary>
        /// <param name="interest">InterestAndContact</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Interest.InterestSendInterestAndContactOnEstate.NoContentException">No Content</exception>
        Task InterestSendInterestAndContactOnEstate(
            InterestOnEstateAndContact interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm&#228;lan till m&#228;klarsystemet f&#246;r ett eller flera omr&#229;den. Skickar in intresseanm&#228;lan till m&#228;klarsystemet f&#246;r ett eller flera omr&#229;den.
        /// </summary>
        /// <param name="interest"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Interest.InterestSendInterestOnArea.NoContentException">No Content</exception>
        Task InterestSendInterestOnArea(
            InterestOnArea interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skickar in intresseanm&#228;lan f&#246;r ett eller flera omr&#229;den, tillsammans med kontaktuppgifter. Innan nya personer l&#228;ggs in i m&#228;klarsystemet, g&#246;rs alltid en dubblettkontroll.
        /// </summary>
        /// <param name="interest"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Interest.InterestSendInterestAndContactOnArea.NoContentException">No Content</exception>
        Task InterestSendInterestAndContactOnArea(
            InterestAndContactOnArea interest,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}