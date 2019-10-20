using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Assessment {
        /// <summary>
        /// Taxeringsv�rden (lista)
        /// </summary>
        [DataMember(Name="assessValues",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.FarmInfo.Estate.AssessValue> AssessValues { get; set; }
    
        /// <summary>
        /// Typkod
        /// </summary>
        [DataMember(Name="typeCode",EmitDefaultValue=false)]
        public string TypeCode { get; set; }
    
        /// <summary>
        /// Prelimin�rt taxeringsv�rde
        /// </summary>
        [DataMember(Name="preliminaryValue",EmitDefaultValue=false)]
        public bool PreliminaryValue { get; set; }
    
        /// <summary>
        /// Taxerings�r
        /// </summary>
        [DataMember(Name="taxYear",EmitDefaultValue=false)]
        public int TaxYear { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
        /// <summary>
        /// Skatt/avgift
        /// </summary>
        [DataMember(Name="taxFee",EmitDefaultValue=false)]
        public double TaxFee { get; set; }
    
    }
}