using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SalesReportResponse {
        /// <summary>
        /// Kontoren som beg�rts
        /// </summary>
        [DataMember(Name="officeSaleReports",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.OfficeYearlyReport> OfficeSaleReports { get; set; }
    
    }
}