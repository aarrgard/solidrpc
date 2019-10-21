using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OfficeYearReport {
        /// <summary>
        /// &#197;r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public string Year { get; set; }
    
        /// <summary>
        /// M&#229;nader
        /// </summary>
        [DataMember(Name="months",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense.OfficeMonthReport> Months { get; set; }
    
    }
}