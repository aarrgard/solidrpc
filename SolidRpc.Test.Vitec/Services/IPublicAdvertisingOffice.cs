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
    public interface IPublicAdvertisingOffice {
        /// <summary>
        /// H&#228;mtar f&#246;retag.
        /// H&#228;mtar information om m&#228;klarkontoret. F&#246;r att kunna h&#228;mta f&#246;retagen s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Office>> PublicAdvertisingOfficeGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar f&#246;retag.
        /// H&#228;mtar information om m&#228;klarkontoret. F&#246;r att kunna h&#228;mta f&#246;retagen s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="officeId"></param>
        /// <param name="cancellationToken"></param>
        Task<Office> PublicAdvertisingOfficeGet(
            string customerId,
            string officeId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}