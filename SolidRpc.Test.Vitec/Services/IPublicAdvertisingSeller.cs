using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingSeller {
        /// <summary>
        /// H�mta mottagare till faktura, g�r det inte att g�ra en bra bed�mning av en mottagare s� returneras ingen mottagare.
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