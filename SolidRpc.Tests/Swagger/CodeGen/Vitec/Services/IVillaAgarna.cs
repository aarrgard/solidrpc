using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Villavardet.Models;
using System;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IVillaAgarna {
        /// <summary>
        /// H�mtar bostadsinformation till Villa�garna
        /// </summary>
        /// <param name="customerId">KundId</param>
        /// <param name="criteriaAccessDateFrom">Tilltr�desdatum fr�n och med</param>
        /// <param name="criteriaAccessDateTo">Tilltr�desdatum till och med</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<SoldEstate>> VillaAgarnaGetObjectsBetween(
            string customerId,
            DateTimeOffset criteriaAccessDateFrom = default(DateTimeOffset),
            DateTimeOffset criteriaAccessDateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}