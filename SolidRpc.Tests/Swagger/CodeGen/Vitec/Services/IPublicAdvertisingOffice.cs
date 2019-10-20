using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingOffice {
        /// <summary>
        /// H�mtar f�retag.
        /// H�mtar information om m�klarkontoret. F�r att kunna h�mta f�retagen s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Office>> PublicAdvertisingOfficeGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar f�retag.
        /// H�mtar information om m�klarkontoret. F�r att kunna h�mta f�retagen s� kr�vs det en giltig API nyckel och ett kundid.
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