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
        /// H&#228;mtar bostadsinformation till Villa&#228;garna
        /// </summary>
        /// <param name="customerId">KundId</param>
        /// <param name="criteriaAccessDateFrom">Tillträdesdatum från och med</param>
        /// <param name="criteriaAccessDateTo">Tillträdesdatum till och med</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<SoldEstate>> VillaAgarnaGetObjectsBetween(
            string customerId,
            DateTimeOffset? criteriaAccessDateFrom = null,
            DateTimeOffset? criteriaAccessDateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}