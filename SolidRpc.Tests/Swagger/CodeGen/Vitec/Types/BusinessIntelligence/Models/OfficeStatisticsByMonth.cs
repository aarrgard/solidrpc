using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OfficeStatisticsByMonth {
        /// <summary>
        /// Lista p� statistik per kontor
        /// </summary>
        [DataMember(Name="officeStatistics",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models.CustomerStatistics> OfficeStatistics { get; set; }
    
    }
}