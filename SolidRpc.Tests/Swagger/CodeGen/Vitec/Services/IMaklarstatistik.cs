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
        /// H�mtar bostadsinformation till m�klarstatistik
        /// </summary>
        /// <param name="customerId">KundId</param>
        /// <param name="criteriaChangeAtFrom">�ndringsdatum fr�n och med</param>
        /// <param name="criteriaChangeAtTo">�ndringsdatum till och med</param>
        /// <param name="criteriaContractDateFrom">Kontraktsdatum fr�n och med, max tre m�nader tidigare �n ContractDateTo</param>
        /// <param name="criteriaContractDateTo">Kontraktsdatum till och med, max tre m�nader senare �n ContractDateFrom</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<PropertyStatistics>> MaklarstatistikGetObjectsBetween(
            string customerId,
            DateTimeOffset criteriaChangeAtFrom = default(DateTimeOffset),
            DateTimeOffset criteriaChangeAtTo = default(DateTimeOffset),
            DateTimeOffset criteriaContractDateFrom = default(DateTimeOffset),
            DateTimeOffset criteriaContractDateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}