using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Estates.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OfficeYearlyReport {
        /// <summary>
        /// Kund-id f�r detta kontor
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Namn p� kontoret
        /// </summary>
        [DataMember(Name="officeName",EmitDefaultValue=false)]
        public string OfficeName { get; set; }
    
        /// <summary>
        /// �r
        /// </summary>
        [DataMember(Name="years",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Estates.BusinessIntelligense.OfficeYearReport> Years { get; set; }
    
    }
}