using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Test.Vitec.Types.Office.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IOffice {
        /// <summary>
        /// H&#228;mtar f&#246;retag.
        /// H&#228;mtar information om m&#228;klarkontoret. F&#246;r att kunna h&#228;mta f&#246;retagen s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaCustomerId">Kontorsid</param>
        /// <param name="criteriaOfficeId">Unik identifierare på butiken</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Office>> OfficeGetOffice(
            string criteriaCustomerId,
            string criteriaOfficeId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}