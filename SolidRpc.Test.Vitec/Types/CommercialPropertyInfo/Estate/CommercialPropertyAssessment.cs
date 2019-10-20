using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyAssessment {
        /// <summary>
        /// Taxeringsv�rden (lista)
        /// </summary>
        [DataMember(Name="assessValues",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CommercialPropertyAssessValue> AssessValues { get; set; }
    
        /// <summary>
        /// Typkod
        /// </summary>
        [DataMember(Name="typeCode",EmitDefaultValue=false)]
        public string TypeCode { get; set; }
    
        /// <summary>
        /// Prelimin�rt taxeringsv�rde
        /// </summary>
        [DataMember(Name="preliminaryAssessedValue",EmitDefaultValue=false)]
        public bool PreliminaryAssessedValue { get; set; }
    
        /// <summary>
        /// Taxerings�r
        /// </summary>
        [DataMember(Name="taxYear",EmitDefaultValue=false)]
        public int TaxYear { get; set; }
    
        /// <summary>
        /// Skatt/avgift
        /// </summary>
        [DataMember(Name="taxFee",EmitDefaultValue=false)]
        public double TaxFee { get; set; }
    
    }
}