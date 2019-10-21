using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OfficeMonthReport {
        /// <summary>
        /// M&#229;nad
        /// </summary>
        [DataMember(Name="month",EmitDefaultValue=false)]
        public string Month { get; set; }
    
        /// <summary>
        /// M&#229;nader
        /// </summary>
        [DataMember(Name="estateSaleReports",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Estates.BusinessIntelligense.EstateSaleReport> EstateSaleReports { get; set; }
    
    }
}