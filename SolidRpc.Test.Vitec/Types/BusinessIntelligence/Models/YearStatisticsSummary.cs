using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligence.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class YearStatisticsSummary {
        /// <summary>
        /// Aktuellt &#229;r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public int Year { get; set; }
    
        /// <summary>
        /// Lista p&#229; statestik per m&#229;nad
        /// </summary>
        [DataMember(Name="months",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.BusinessIntelligence.Models.MonthStatisticsSummary> Months { get; set; }
    
    }
}