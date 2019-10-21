using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingSeller {
        /// <summary>
        /// H&#228;mta mottagare till faktura, g&#229;r det inte att g&#246;ra en bra bed&#246;mning av en mottagare s&#229; returneras ingen mottagare.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="estateId"></param>
        /// <param name="cancellationToken"></param>
        Task<InvoiceRecipient> PublicAdvertisingSellerGetInvoiceRecipient(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}