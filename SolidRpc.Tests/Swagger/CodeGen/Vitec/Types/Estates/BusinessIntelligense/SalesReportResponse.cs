using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SalesReportResponse {
        /// <summary>
        /// Kontoren som beg&#228;rts
        /// </summary>
        [DataMember(Name="officeSaleReports",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense.OfficeYearlyReport> OfficeSaleReports { get; set; }
    
    }
}