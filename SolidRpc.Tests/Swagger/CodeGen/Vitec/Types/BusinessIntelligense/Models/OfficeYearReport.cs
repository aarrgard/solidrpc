using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OfficeYearReport {
        /// <summary>
        /// �r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public string Year { get; set; }
    
        /// <summary>
        /// M�nader
        /// </summary>
        [DataMember(Name="months",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.OfficeMonthReport> Months { get; set; }
    
    }
}