using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingAssociation {
        /// <summary>
        /// H�mtar information om bostadsr�ttsf�reningar. F�r att kunna h�mta bostadsr�ttsf�reningar s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Association>> PublicAdvertisingAssociationGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar information om bostadsr�ttsf�reningen. F�r att kunna h�mta bostadsr�ttsf�reningen s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="associationId"></param>
        /// <param name="cancellationToken"></param>
        Task<Association> PublicAdvertisingAssociationGet(
            string customerId,
            string associationId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}