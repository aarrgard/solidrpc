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
        /// H&#228;mtar information om bostadsr&#228;ttsf&#246;reningar. F&#246;r att kunna h&#228;mta bostadsr&#228;ttsf&#246;reningar s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Association>> PublicAdvertisingAssociationGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar information om bostadsr&#228;ttsf&#246;reningen. F&#246;r att kunna h&#228;mta bostadsr&#228;ttsf&#246;reningen s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
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