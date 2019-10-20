using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class YearStatisticsSummary {
        /// <summary>
        /// Aktuellt �r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public int Year { get; set; }
    
        /// <summary>
        /// Lista p� statestik per m�nad
        /// </summary>
        [DataMember(Name="months",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models.MonthStatisticsSummary> Months { get; set; }
    
    }
}