using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Maklarstatistik.Models;
using System;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMaklarstatistik {
        /// <summary>
        /// H&#228;mtar bostadsinformation till m&#228;klarstatistik
        /// </summary>
        /// <param name="customerId">KundId</param>
        /// <param name="criteriaChangeAtFrom">Ändringsdatum från och med</param>
        /// <param name="criteriaChangeAtTo">Ändringsdatum till och med</param>
        /// <param name="criteriaContractDateFrom">Kontraktsdatum från och med, max tre månader tidigare än ContractDateTo</param>
        /// <param name="criteriaContractDateTo">Kontraktsdatum till och med, max tre månader senare än ContractDateFrom</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<PropertyStatistics>> MaklarstatistikGetObjectsBetween(
            string customerId,
            System.DateTimeOffset? criteriaChangeAtFrom = null,
            System.DateTimeOffset? criteriaChangeAtTo = null,
            System.DateTimeOffset? criteriaContractDateFrom = null,
            System.DateTimeOffset? criteriaContractDateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}