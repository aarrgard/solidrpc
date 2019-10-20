using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Office.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IOffice {
        /// <summary>
        /// H�mtar f�retag.
        /// H�mtar information om m�klarkontoret. F�r att kunna h�mta f�retagen s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaCustomerId">Kontorsid</param>
        /// <param name="criteriaOfficeId">Unik identifierare p� butiken</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Office>> OfficeGetOffice(
            string criteriaCustomerId,
            string criteriaOfficeId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}